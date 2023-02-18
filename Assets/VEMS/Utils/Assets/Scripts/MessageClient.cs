using System.Collections.Generic;
using UnityEngine;
using MQTTnet;
using MQTTnet.Client;
using System;

namespace ubc.ok.VEMS.Utils
{
    public class MessageClient: MonoBehaviour
    {
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

        [Tooltip("The server address of the MQTT broker.")]
        public string serverAddress = "127.0.0.1";
        [Tooltip("The port of the MQTT broker.")]
        public int port = 80;

        private Dictionary<string, List<Action<string>>> subsciptions = new Dictionary<string, List<Action<string>>>();
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
        /// <example>
        ///   <code>
        ///     MessageClient.Instance.AddSubscriptions(new Dictionary<string, Action<string>>()
        ///     {
        ///         {"/myCustomMessage/messageX", payload => {Debug.Log(payload);}}
        ///     });
        ///   </code>
        /// </example>
        /// </summary>
        /// <param name="subscribingTopicHandlers">
        ///    A dictionary, where a given key is the topic and the value of
        ///    the key is a System.Action that takes the payload (string) as a parameter.
        /// </param>
        public void AddSubscriptions(Dictionary<string, Action<string>> subscribingTopicHandlers)
        {
            foreach (var kvp in subscribingTopicHandlers)
            {
                string topic = kvp.Key;

                if (!subsciptions.ContainsKey(topic))
                {
                    subsciptions[topic] = new List<Action<string>>();
                }

                subsciptions[topic].Add(kvp.Value);
            }
        }

        /// <summary>
        /// The topics to subscribe to and the corresponding handlers when a message of the topic is recieved.
        /// </summary>
        /// <param name="subscribingTopicHandlers">
        ///    A dictionary, where a given key is the topic and the value of
        ///    the key is a collection of System.Action that takes the payload (string) as a parameter.
        /// </param>
        public void AddSubscriptions(Dictionary<string, IEnumerable<Action<string>>> subscribingTopicHandlers)
        {
            foreach (var kvp in subscribingTopicHandlers)
            {
                string topic = kvp.Key;

                if (!subsciptions.ContainsKey(topic))
                {
                    subsciptions[topic] = new List<Action<string>>();
                }

                foreach (Action<string> handler in kvp.Value)
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

            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("127.0.0.1", 80).Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                string payload = e.ApplicationMessage.ConvertPayloadToString();
                string topic = e.ApplicationMessage.Topic;
                if (subsciptions.ContainsKey(topic))
                {
                    subsciptions[topic].ForEach(handler => handler.Invoke(payload));
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
            foreach (string topic in subsciptions.Keys)
            {
                mqttSubscribeOptionsBuilder = mqttSubscribeOptionsBuilder.WithTopicFilter(f => { f.WithTopic(topic); });
            }

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
    }
}
