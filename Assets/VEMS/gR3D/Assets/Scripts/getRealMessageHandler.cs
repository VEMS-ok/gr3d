using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using getReal3D;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using System.Threading.Tasks;
using System;

namespace ubc.ok.VEMS.gr3d
{
    public class getRealMessageHandler : getReal3D.MonoBehaviourWithRpc, PlayerInputs
    {
        private float yawAxis = 0;
        private float pitchAxis = 0;
        private bool wandLookButtonDown = false;
        private bool wandLookButtonUp = false;
        private bool wandLookButton = false;
        private bool wandDriveButtonDown = false;
        private bool wandDriveButtonUp = false;
        private bool wandDriveButton = false;
        private float strafeAxis = 0;
        private float forwardAxis = 0;
        private bool navSpeedButtonDown = false;
        private bool navSpeedButtonUp = false;
        private bool navSpeedButton = false;
        private bool jumpButtonDown = false;
        private bool jumpButtonUp = false;
        private bool jumpButton = false;
        private bool wandButtonDown = false;
        private bool wandButtonUp = false;
        private bool wandButton = false;
        private bool changeWandButtonDown = false;
        private bool changeWandButtonUp = false;
        private bool changeWandButton = false;
        private bool resetButtonDown = false;
        private bool resetButtonUp = false;
        private bool resetButton = false;
        private Sensor wand = new Sensor();
        private Sensor head = new Sensor();

        public MonoBehaviour behaviour => GetBehaviour();

        public float YawAxis => GetYawAxis();

        public float PitchAxis => GetPitchAxis();

        public bool WandLookButtonDown => GetWandLookButtonDown();

        public bool WandLookButtonUp => GetWandLookButtonUp();

        public bool WandLookButton => GetWandLookButton();

        public bool WandDriveButtonDown => GetWandDriveButtonDown();

        public bool WandDriveButtonUp => GetWandDriveButtonUp();

        public bool WandDriveButton => GetWandDriveButton();

        public float StrafeAxis => GetStrafeAxis();

        public float ForwardAxis => GetForwardAxis();

        public bool NavSpeedButtonDown => GetNavSpeedButtonDown();

        public bool NavSpeedButtonUp => GetNavSpeedButtonUp();

        public bool NavSpeedButton => GetNavSpeedButton();

        public bool JumpButtonDown => GetJumpButtonDown();

        public bool JumpButtonUp => GetJumpButtonUp();

        public bool JumpButton => GetJumpButton();

        public bool WandButtonDown => GetWandButtonDown();

        public bool WandButtonUp => GetWandButtonUp();

        public bool WandButton => GetWandButton();

        public bool ChangeWandButtonDown => GetChangeWandButtonDown();

        public bool ChangeWandButtonUp => GetChangeWandButtonUp();

        public bool ChangeWandButton => GetChangeWandButton();

        public bool ResetButtonDown => GetResetButtonDown();

        public bool ResetButtonUp => GetResetButtonUp();

        public bool ResetButton => GetResetButton();

        public Sensor Wand => GetWand();

        public Sensor Head => GetHead();

    #region Unity functions
        void Start()
        {
            // if(getReal3D.Cluster.isMaster)
            // {
            //     // CallRpc("SetupMessageHandlers");
            // }
            SetupMessageHandlers();
        }

        void Update()
        {
        }
    #endregion

    #region PlayerInput functions
        public MonoBehaviour GetBehaviour()
        {
            return this;
        }

        public float GetYawAxis()
        {
            float retVal = yawAxis;
            // yawAxis = 0;
            return retVal;
        }

        public float GetPitchAxis()
        {
            float retVal = pitchAxis;
            // pitchAxis = 0;
            return retVal;
        }

        public bool GetWandLookButtonDown()
        {
            return wandLookButtonDown;
        }

        public bool GetWandLookButtonUp()
        {
            return wandLookButtonUp;
        }

        public bool GetWandLookButton()
        {
            bool retVal = wandLookButton;
            // wandLookButton = false;
            return retVal;
        }

        public bool GetWandDriveButtonDown()
        {
            return wandDriveButtonDown;
        }

        public bool GetWandDriveButtonUp()
        {
            return wandDriveButtonUp;
        }

        public bool GetWandDriveButton()
        {
            bool retVal = wandDriveButton;
            // wandDriveButton = false;
            return retVal;
        }

        public float GetStrafeAxis()
        {
            float retVal = strafeAxis;
            // strafeAxis = 0;
            return retVal;
        }

        public float GetForwardAxis()
        {
            float retVal = forwardAxis;
            // forwardAxis = 0;
            return retVal;
        }

        public bool GetNavSpeedButtonDown()
        {
            return navSpeedButtonDown;
        }

        public bool GetNavSpeedButtonUp()
        {
            return navSpeedButtonUp;
        }

        public bool GetNavSpeedButton()
        {
            bool retVal = navSpeedButton;
            // navSpeedButton = false;
            return retVal;
        }

        public bool GetJumpButtonDown()
        {
            return jumpButtonDown;
        }

        public bool GetJumpButtonUp()
        {
            return jumpButtonUp;
        }

        public bool GetJumpButton()
        {
            bool retVal = jumpButton;
            // jumpButton = false;
            return retVal;
        }

        public bool GetWandButtonDown()
        {
            return wandButtonDown;
        }

        public bool GetWandButtonUp()
        {
            return wandButtonUp;
        }

        public bool GetWandButton()
        {
            bool retVal = wandButton;
            // wandButton = false;
            return retVal;
        }

        public bool GetChangeWandButtonDown()
        {
            return changeWandButtonDown;
        }

        public bool GetChangeWandButtonUp()
        {
            return changeWandButtonUp;
        }

        public bool GetChangeWandButton()
        {
            bool retVal = changeWandButton;
            // changeWandButton = false;
            return retVal;
        }

        public bool GetResetButtonDown()
        {
            return resetButtonDown;
        }

        public bool GetResetButtonUp()
        {
            return resetButtonUp;
        }

        public bool GetResetButton()
        {
            bool retVal = resetButton;
            // resetButton = false;
            return retVal;
        }

        public Sensor GetWand()
        {
            return wand;
        }

        public Sensor GetHead()
        {
            return head;
        }
    #endregion

    #region MQTT
        async void SetupMessageHandlers()
        {
            MqttFactory mqttFactory = new MqttFactory();

            IMqttClient mqttClient = mqttFactory.CreateMqttClient();
        
            MqttClientOptions mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("127.0.0.1", 80).Build();

            mqttClient.ApplicationMessageReceivedAsync += e =>
            {
                // Before processing any messages, reset values
                ResetValues();
                string payload = e.ApplicationMessage.ConvertPayloadToString();
                switch (e.ApplicationMessage.Topic)
                {
                    case "/getReal3D/yawAxis":
                        SetYawAxis(payload);
                        break;
                    case "/getReal3D/pitchAxis":
                        SetPitchAxis(payload);
                        break;
                    case "/getReal3D/strafeAxis":
                        SetStrafeAxis(payload);
                        break;
                    case "/getReal3D/forwardAxis":
                        SetForwardAxis(payload);
                        break;
                    case "/getReal3D/wandLookButtonDown":
                        SetWandLookButtonDown(payload);
                        break;
                    case "/getReal3D/wandLookButtonUp":
                        SetWandLookButtonUp(payload);
                        break;
                    case "/getReal3D/wandDriveButtonDown":
                        SetWandDriveButtonDown(payload);
                        break;
                    case "/getReal3D/wandDriveButtonUp":
                        SetWandDriveButtonUp(payload);
                        break;
                    case "/getReal3D/navSpeedButtonDown":
                        SetNavSpeedButtonDown(payload);
                        break;
                    case "/getReal3D/navSpeedButtonUp":
                        SetNavSpeedButtonUp(payload);
                        break;
                    case "/getReal3D/jumpButtonDown":
                        SetJumpButtonDown(payload);
                        break;
                    case "/getReal3D/jumpButtonUp":
                        SetJumpButtonUp(payload);
                        break;
                    case "/getReal3D/wandButtonDown":
                        SetWandButtonDown(payload);
                        break;
                    case "/getReal3D/wandButtonUp":
                        SetWandButtonUp(payload);
                        break;
                    case "/getReal3D/changeWandButtonDown":
                        SetChangeWandButtonDown(payload);
                        break;
                    case "/getReal3D/changeWandButtonUp":
                        SetChangeWandButtonUp(payload);
                        break;
                    case "/getReal3D/resetButtonDown":
                        SetResetButtonDown(payload);
                        break;
                    case "/getReal3D/resetButtonUp":
                        SetResetButtonUp(payload);
                        break;
                }
                return System.Threading.Tasks.Task.CompletedTask;
            };

            await mqttClient.ConnectAsync(mqttClientOptions);

            // Create the subscribe options including several topics with different options.
            // It is also possible to all of these topics using a dedicated call of _SubscribeAsync_ per topic.
            MqttClientSubscribeOptions mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/yawAxis");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/pitchAxis");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/strafeAxis");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/forwardAxis");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/wandLookButtonDown");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/wandLookButtonUp");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/wandDriveButtonDown");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/wandDriveButtonUp");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/navSpeedButtonDown");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/navSpeedButtonUp");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/jumpButtonDown");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/jumpButtonUp");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/wandButtonDown");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/wandButtonUp");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/changeWandButtonDown");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/changeWandButtonUp");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/resetButtonDown");})
                .WithTopicFilter(f => {f.WithTopic("/getReal3D/resetButtonUp");})
                .Build();

            MqttClientSubscribeResult response = await mqttClient.SubscribeAsync(mqttSubscribeOptions);

            Debug.Log("MQTT client subscribed to topics.");

            // The response contains additional data sent by the server after subscribing.
            // Debug.Log($"{response.ReasonString} " + string.Join(", ",response.Items));
        }


    #region helper functions
        private float StringToFloat(string message)
        {
            return float.Parse(message);
        }

        private bool StringToBool(string message)
        {
            return bool.Parse(message);
        }

        private void SetYawAxis(string message)
        {
            yawAxis = StringToFloat(message);
        }

        private void SetPitchAxis(string message)
        {
            pitchAxis = StringToFloat(message);
        }

        private void SetStrafeAxis(string message)
        {
            strafeAxis = StringToFloat(message);
        }
    
        private void SetForwardAxis(string message)
        {
            forwardAxis = StringToFloat(message);
        }

        private void SetWandLookButtonDown(string message)
        {
            wandLookButtonDown = true;
            wandLookButtonUp = false;
            wandLookButton = wandLookButtonDown;
        }

        private void SetWandLookButtonUp(string message)
        {
            wandLookButtonDown = false;
            wandLookButtonUp = true;
            wandLookButton = false;
        }

        private void SetWandDriveButtonDown(string message)
        {
            wandDriveButtonDown = true;
            wandDriveButtonUp = false;
            wandDriveButton = false;
        }

        private void SetWandDriveButtonUp(string message)
        {
            wandDriveButtonDown = false;
            wandDriveButtonUp = true;
            wandDriveButton = false;
        }

        private void SetNavSpeedButtonDown(string message)
        {
            navSpeedButtonDown = true;
            navSpeedButtonUp = false;
            navSpeedButton = navSpeedButtonDown;
        }

        private void SetNavSpeedButtonUp(string message)
        {
            navSpeedButtonDown = false;
            navSpeedButtonUp = true;
            navSpeedButton = false;
        }

        private void SetJumpButtonDown(string message)
        {
            jumpButtonDown = true;
            jumpButtonUp = false;
            jumpButton = jumpButtonDown;
        }

        private void SetJumpButtonUp(string message)
        {
            jumpButtonDown = false;
            jumpButtonUp = true;
            jumpButton = false;
        }

        private void SetWandButtonDown(string message)
        {
            wandButtonDown = true;
            wandButtonUp = false;
            wandButton = wandButtonDown;
        }

        private void SetWandButtonUp(string message)
        {
            wandButtonDown = false;
            wandButtonUp = true;
            wandButton = false;
        }

        private void SetChangeWandButtonDown(string message)
        {
            changeWandButtonDown = true;
            changeWandButtonUp = false;
            changeWandButton = changeWandButtonDown;
        }

        private void SetChangeWandButtonUp(string message)
        {
            changeWandButtonDown = false;
            changeWandButtonUp = true;
            changeWandButton = false;
        }

        private void SetResetButtonDown(string message)
        {
            resetButtonDown = true;
            resetButtonUp = false;
            resetButton = resetButtonDown;
        }

        private void SetResetButtonUp(string message)
        {
            resetButtonDown = false;
            resetButtonUp = true;
            resetButton = false;
        }

        private void ResetValues()
        {
            yawAxis = 0;
            pitchAxis = 0;
            wandLookButtonDown = false;
            wandLookButtonUp = false;
            wandDriveButtonDown = false;
            wandDriveButtonUp = false;
            strafeAxis = 0;
            forwardAxis = 0;
            navSpeedButtonDown = false;
            navSpeedButtonUp = false;
            jumpButtonDown = false;
            jumpButtonUp = false;
            wandButtonDown = false;
            wandButtonUp = false;
            changeWandButtonDown = false;
            changeWandButtonUp = false;
            resetButtonDown = false;
            resetButtonUp = false;
        }
    #endregion

    #endregion
    }
}
