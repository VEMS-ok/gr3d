using System.Collections.Generic;
using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using System;

namespace ubc.ok.VEMS.Utils
{
    /// <summary>
    /// This class allows connecting to an MQTTServer and await messages. This is also a singleton, which allows accessing an
    /// instance of this from anywhere in code. For this to work, it needs to be added as a component within the scene.
    /// The `Player` prefab has a MessageClient configured which can be accessed with `MessageClient.Instance` 
    ///
    /// The `testServerAddress` and `testPort` can be set to use a test server while developing in Editor, once built
    /// it will use predefined servers of VEMS.
    /// </summary>
    public class MessageClient: MonoBehaviour
    {
        // NOTE: hardcoding these as these are not expected to change!
        private const Protocol BROKER_PROTOCOL = Protocol.wss;
        private const string BROKER_ADDRESS = "vemslab.ok.ubc.ca";
        private const int BROKER_PORT = 80;

        private static MessageClient instance = null;
        public static MessageClient Instance {
            get {
                if (instance == null)
                {
                    MessageClient[] instances = FindObjectsOfType<MessageClient>();
                    if (instances.Length != 1)
                    {
                        throw new Exception("The scene must one MessageClient. Got " + instances.Length);
                    }

                    instance = instances[0];
                }
                return instance;
            }
        }

        [Tooltip("The server address of the test MQTT broker. Only used when playing from Editor. Uese preconfigured value when built.")]
        /// <summary>
        /// The server address of the test MQTT broker. Only used when playing from Editor. Uese preconfigured value when built.
        /// </summary>
        public string testServerAddress = "127.0.0.1";
        [Tooltip("The port of the test MQTT broker. Only used when playing from Editor. Uese preconfigured value when built.")]
        /// <summary>
        /// The port of the test MQTT broker. Only used when playing from Editor. Uese preconfigured value when built.
        /// </summary>
        public int testPort = 80;

        [Tooltip("The protocol to use with the test MQTT broker. Only used when playing from Editor. Uese preconfigured value when built.")]
        /// <summary>
        /// The protocol to use with the test MQTT broker. Only used when playing from Editor. Uese preconfigured value when built.
        /// </summary>
        public Protocol testProtocol = Protocol.mqtt;

        private Dictionary<string, List<Action<string, string>>> subsciptions = new Dictionary<string, List<Action<string, string>>>();
        IMqttClient mqttClient;

        void Start()
        {
            ConnectToServer();
        }

        void OnDestroy()
        {
            if (mqttClient != null)
            {
                mqttClient.DisconnectAsync();
            }
        }

        /// <summary>
        /// The topics to subscribe to and the corresponding handlers when a message of the topic is recieved.
        ///
        /// FIXME: Currently doesn't handle wildcards in topics.
        ///
        /// <example>
        /// @code
        ///   MessageClient.Instance.AddSubscriptions(new Dictionary<string, Action<string>>()
        ///   {
        ///       {"/myCustomMessage/messageX", (topic, payload) => {Debug.Log(payload);}}
        ///   });
        /// @endcode
        /// </example>
        /// </summary>
        /// <param name="subscribingTopicHandlers">
        ///    A dictionary, where a given key is the topic and the value of
        ///    the key is a System.Action that takes the topic (string) and payload (string) as a parameter.
        /// </param>
        public void AddSubscriptions(Dictionary<string, Action<string, string>> subscribingTopicHandlers)
        {
            foreach (var kvp in subscribingTopicHandlers)
            {
                string topic = kvp.Key;

                if (!subsciptions.ContainsKey(topic))
                {
                    subsciptions[topic] = new List<Action<string, string>>();
                }

                subsciptions[topic].Add(kvp.Value);
            }
        }

        /// <summary>
        /// The topics to subscribe to and the corresponding handlers when a message of the topic is recieved.
        ///
        /// FIXME: Currently doesn't handle wildcards in topics
        ///
        /// </summary>
        /// <param name="subscribingTopicHandlers">
        ///    A dictionary, where a given key is the topic and the value of
        ///    the key is a collection of System.Action that takes the topic (string) and payload (string) as a parameter.
        /// </param>
        public void AddSubscriptions(Dictionary<string, IEnumerable<Action<string, string>>> subscribingTopicHandlers)
        {
            foreach (var kvp in subscribingTopicHandlers)
            {
                string topic = kvp.Key;

                if (!subsciptions.ContainsKey(topic))
                {
                    subsciptions[topic] = new List<Action<string, string>>();
                }

                foreach (Action<string, string> handler in kvp.Value)
                {
                    subsciptions[topic].Add(handler);
                }
            }
        }

        /// <summary>
        /// Connect to MQTT broker. Handles all topics and handlers configured in subscriptions.
        /// </summary>
        private async void ConnectToServer()
        {
            MqttFactory mqttFactory = new MqttFactory();

            mqttClient = mqttFactory.CreateMqttClient();

            string _serverAddress;
            int _port;
            string _protocol;

#if UNITY_EDITOR
            _protocol = testProtocol.ToString();
            _serverAddress = testServerAddress;
            _port = testPort;

            if (testServerAddress.Equals(BROKER_ADDRESS))
            {
                Debug.Log("The Test Server Address cannot be set to " + BROKER_ADDRESS);
                return;
            }
#else
            _protocol = BROKER_PROTOCOL.ToString();
            _serverAddress = BROKER_ADDRESS;
            _port = BROKER_PORT;
#endif

            string address = $"{_protocol}://{_serverAddress}:{_port}";
            Debug.Log($"Connecting to {address}");
            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder()
                .WithConnectionUri($"{address}")
                .WithCleanSession()
                .Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                string payload = e.ApplicationMessage.ConvertPayloadToString();
                string topic = e.ApplicationMessage.Topic;
                if (subsciptions.ContainsKey(topic))
                {
                    subsciptions[topic].ForEach(handler => handler.Invoke(topic, payload));
                }
                else
                {
                    Debug.LogWarning($"Unhandler topic: " + topic);
                }
                
                return System.Threading.Tasks.Task.CompletedTask;
            };

            try
            {
                await mqttClient.ConnectAsync(mqttClientOptions);
            }
            catch (Exception e)
            {
                Debug.LogError("Connection failed:" + e.Message);
                return;
            }

            // Create the subscribe options including several topics with different options.
            // It is also possible to all of these topics using a dedicated call of _SubscribeAsync_ per topic.
            MqttClientSubscribeOptionsBuilder mqttSubscribeOptionsBuilder = mqttFactory.CreateSubscribeOptionsBuilder();
            mqttSubscribeOptionsBuilder = mqttSubscribeOptionsBuilder.WithTopicFilter(f => { f.WithTopic("#"); });

            MqttClientSubscribeOptions mqttSubscribeOptions = mqttSubscribeOptionsBuilder.Build();

            try
            {
                MqttClientSubscribeResult response = await mqttClient.SubscribeAsync(mqttSubscribeOptions);
            }
            catch(Exception e)
            {
                Debug.LogError("Subscribing to broker filed with error: " + e.Message);
            }

            Debug.Log("MQTT client subscribed to topics.");
            // The response contains additional data sent by the server after subscribing.
            // Debug.Log($"{response.ReasonString} " + string.Join(", ",response.Items));
        }

        [Serializable]
        public enum Protocol{
            ws, wss, mqtt
        }
    }
}
