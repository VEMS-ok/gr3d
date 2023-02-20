using System.Collections.Generic;
using UnityEngine;
using getReal3D;
using System;
using ubc.ok.VEMS.Utils;

namespace ubc.ok.VEMS.gr3d
{
    /// <summary>
    /// A PlayerInput that can recieves input remotly using MessageClient. Implements the PlayerInputs interface,
    /// hence, can be used as a dropin replacemement for getReal3DPlayerInput. Subscribs to the following message:
    /// - "{rootTopic}/yawAxis"
    /// - "{rootTopic}/pitchAxis"
    /// - "{rootTopic}/strafeAxis"
    /// - "{rootTopic}/forwardAxis"
    /// - "{rootTopic}/wandLookButtonDown"
    /// - "{rootTopic}/wandLookButtonUp"
    /// - "{rootTopic}/wandDriveButtonDown"
    /// - "{rootTopic}/wandDriveButtonUp"
    /// - "{rootTopic}/navSpeedButtonDown"
    /// - "{rootTopic}/navSpeedButtonUp"
    /// - "{rootTopic}/jumpButtonDown"
    /// - "{rootTopic}/jumpButtonUp"
    /// - "{rootTopic}/wandButtonDown"
    /// - "{rootTopic}/wandButtonUp"
    /// - "{rootTopic}/changeWandButtonDown"
    /// - "{rootTopic}/changeWandButtonUp"
    /// - "{rootTopic}/resetButtonDown"
    /// - "{rootTopic}/resetButtonUp"
    ///
    /// Where "{rootTopic}" is the value set for `rootTopic`. By default this is "getReal3D"
    /// NOTE: Doesn't handle the `Wand` and `Head` fields of the `PlayerInterface`
    ///
    /// If `setupOnStart` is false, the `SetupSubscriptions` method will have to be manually called to subscribe.
    ///
    /// Whenever a new message is recieved or after waiting for 100 ms, the following values are set to the
    /// default values shown:
    ///   YawAxis = 0;
    ///   PitchAxis = 0;
    ///   WandLookButtonDown = false;
    ///   WandLookButtonUp = false;
    ///   WandDriveButtonDown = false;
    ///   WandDriveButtonUp = false;
    ///   StrafeAxis = 0;
    ///   ForwardAxis = 0;
    ///   NavSpeedButtonDown = false;
    ///   NavSpeedButtonUp = false;
    ///   JumpButtonDown = false;
    ///   JumpButtonUp = false;
    ///   WandButtonDown = false;
    ///   WandButtonUp = false;
    ///   ChangeWandButtonDown = false;
    ///   ChangeWandButtonUp = false;
    ///   ResetButtonDown = false;
    ///   ResetButtonUp = false;
    /// </summary>
    public class getRealRemoteInput : getReal3D.MonoBehaviourWithRpc, PlayerInputs
    {
        #region PlayerInput values
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
        #endregion

        private DateTime lastMessaageTime = DateTime.Now;
        // Reset values after 100 ms;
        private TimeSpan resetTime = new TimeSpan(0, 0, 0, 0, 100);
        private bool processingMessage = false;

        [Tooltip("Setup subscription on startup?")]
        /// <summary>
        /// If set to `true`, will call the `SetupSubscription` method within `Start`.
        /// </summary>
        public bool setupOnStart = true;
        [Tooltip("The root topic of this client.")]
        /// <summary>
        /// The root topic of the client.
        /// </summary>
        public string rootTopic = "getReal3D";


    #region Unity functions
        protected void Start()
        {
            // if(getReal3D.Cluster.isMaster)
            // {
            //     // CallRpc("SetupMessageHandlers");
            // }
            if (setupOnStart)
            {
                SetupMessageHandlers();
            }
        }

        protected void Update()
        {
            // If the time lapsed since the last message is above set threshold, reset the values.
            DateTime now = DateTime.Now;
            if ((now - lastMessaageTime) > resetTime)
            {
                ResetValues();
            }
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
        /// <summary>
        /// Subscribes to the topics with the root topic being the value set for `rootTopic`. See the documentation of `getRealRemoteInput` for messages subscribed to.
        /// Note that any message recieved while a previous message is being processed wil be dropped.
        /// </summary>
        public void SetupMessageHandlers()
        {
            Dictionary<string, Action<string, string>> subscriptions = new Dictionary<string, Action<string, string>>()
            {
                {$"{rootTopic}/yawAxis", HandleMessage},
                {$"{rootTopic}/pitchAxis", HandleMessage},
                {$"{rootTopic}/strafeAxis", HandleMessage},
                {$"{rootTopic}/forwardAxis", HandleMessage},
                {$"{rootTopic}/wandLookButtonDown", HandleMessage},
                {$"{rootTopic}/wandLookButtonUp", HandleMessage},
                {$"{rootTopic}/wandDriveButtonDown", HandleMessage},
                {$"{rootTopic}/wandDriveButtonUp", HandleMessage},
                {$"{rootTopic}/navSpeedButtonDown", HandleMessage},
                {$"{rootTopic}/navSpeedButtonUp", HandleMessage},
                {$"{rootTopic}/jumpButtonDown", HandleMessage},
                {$"{rootTopic}/jumpButtonUp", HandleMessage},
                {$"{rootTopic}/wandButtonDown", HandleMessage},
                {$"{rootTopic}/wandButtonUp", HandleMessage},
                {$"{rootTopic}/changeWandButtonDown", HandleMessage},
                {$"{rootTopic}/changeWandButtonUp", HandleMessage},
                {$"{rootTopic}/resetButtonDown", HandleMessage},
                {$"{rootTopic}/resetButtonUp", HandleMessage}
            };

            MessageClient.Instance.AddSubscriptions(subscriptions);
        }


    #region helper functions
        private void HandleMessage(string topic, string payload)
        {
            if (!processingMessage)
            {
                processingMessage = true;
                switch (topic)
                {
                    case "getReal3D/yawAxis":
                        SetYawAxis(payload);
                        break;
                    case "getReal3D/pitchAxis":
                        SetPitchAxis(payload);
                        break;
                    case "getReal3D/strafeAxis":
                        SetStrafeAxis(payload);
                        break;
                    case "getReal3D/forwardAxis":
                        SetForwardAxis(payload);
                        break;
                    case "getReal3D/wandLookButtonDown":
                        SetWandLookButtonDown(payload);
                        break;
                    case "getReal3D/wandLookButtonUp":
                        SetWandLookButtonUp(payload);
                        break;
                    case "getReal3D/wandDriveButtonDown":
                        SetWandDriveButtonDown(payload);
                        break;
                    case "getReal3D/wandDriveButtonUp":
                        SetWandDriveButtonUp(payload);
                        break;
                    case "getReal3D/navSpeedButtonDown":
                        SetNavSpeedButtonDown(payload);
                        break;
                    case "getReal3D/navSpeedButtonUp":
                        SetNavSpeedButtonUp(payload);
                        break;
                    case "getReal3D/jumpButtonDown":
                        SetJumpButtonDown(payload);
                        break;
                    case "getReal3D/jumpButtonUp":
                        SetJumpButtonUp(payload);
                        break;
                    case "getReal3D/wandButtonDown":
                        SetWandButtonDown(payload);
                        break;
                    case "getReal3D/wandButtonUp":
                        SetWandButtonUp(payload);
                        break;
                    case "getReal3D/changeWandButtonDown":
                        SetChangeWandButtonDown(payload);
                        break;
                    case "getReal3D/changeWandButtonUp":
                        SetChangeWandButtonUp(payload);
                        break;
                    case "getReal3D/resetButtonDown":
                        SetResetButtonDown(payload);
                        break;
                    case "getReal3D/resetButtonUp":
                        SetResetButtonUp(payload);
                        break;
                }
                processingMessage = false;
            }
        }
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
            MessageRecieved();
            yawAxis = StringToFloat(message);
        }

        private void SetPitchAxis(string message)
        {
            MessageRecieved();
            pitchAxis = StringToFloat(message);
        }

        private void SetStrafeAxis(string message)
        {
            MessageRecieved();
            strafeAxis = StringToFloat(message);
        }
    
        private void SetForwardAxis(string message)
        {
            MessageRecieved();
            forwardAxis = StringToFloat(message);
        }

        private void SetWandLookButtonDown(string message)
        {
            MessageRecieved();
            wandLookButtonDown = true;
            wandLookButtonUp = false;
            wandLookButton = wandLookButtonDown;
        }

        private void SetWandLookButtonUp(string message)
        {
            MessageRecieved();
            wandLookButtonDown = false;
            wandLookButtonUp = true;
            wandLookButton = false;
        }

        private void SetWandDriveButtonDown(string message)
        {
            MessageRecieved();
            wandDriveButtonDown = true;
            wandDriveButtonUp = false;
            wandDriveButton = false;
        }

        private void SetWandDriveButtonUp(string message)
        {
            MessageRecieved();
            wandDriveButtonDown = false;
            wandDriveButtonUp = true;
            wandDriveButton = false;
        }

        private void SetNavSpeedButtonDown(string message)
        {
            MessageRecieved();
            navSpeedButtonDown = true;
            navSpeedButtonUp = false;
            navSpeedButton = navSpeedButtonDown;
        }

        private void SetNavSpeedButtonUp(string message)
        {
            MessageRecieved();
            navSpeedButtonDown = false;
            navSpeedButtonUp = true;
            navSpeedButton = false;
        }

        private void SetJumpButtonDown(string message)
        {
            MessageRecieved();
            jumpButtonDown = true;
            jumpButtonUp = false;
            jumpButton = jumpButtonDown;
        }

        private void SetJumpButtonUp(string message)
        {
            MessageRecieved();
            jumpButtonDown = false;
            jumpButtonUp = true;
            jumpButton = false;
        }

        private void SetWandButtonDown(string message)
        {
            MessageRecieved();
            wandButtonDown = true;
            wandButtonUp = false;
            wandButton = wandButtonDown;
        }

        private void SetWandButtonUp(string message)
        {
            MessageRecieved();
            wandButtonDown = false;
            wandButtonUp = true;
            wandButton = false;
        }

        private void SetChangeWandButtonDown(string message)
        {
            MessageRecieved();
            changeWandButtonDown = true;
            changeWandButtonUp = false;
            changeWandButton = changeWandButtonDown;
        }

        private void SetChangeWandButtonUp(string message)
        {
            MessageRecieved();
            changeWandButtonDown = false;
            changeWandButtonUp = true;
            changeWandButton = false;
        }

        private void SetResetButtonDown(string message)
        {
            MessageRecieved();
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

        /// <summary>
        /// Before processing any messages, reset values with this function.
        /// </summary>
        protected void ResetValues()
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

        /// <summary>
        /// Method used to indicate the message is received. Sets the time of last message.
        /// </summary>
        protected void MessageRecieved()
        {
            lastMessaageTime = DateTime.Now;
        }
    #endregion

    #endregion
    }
}
