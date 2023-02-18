using System.Collections.Generic;
using UnityEngine;
using getReal3D;
using System;
using ubc.ok.VEMS.Utils;

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
            Dictionary<string, Action<string>> subscriptions = new Dictionary<string, Action<string>>()
            {
                {"/getReal3D/yawAxis", SetYawAxis},
                {"/getReal3D/pitchAxis", SetPitchAxis},
                {"/getReal3D/strafeAxis", SetStrafeAxis},
                {"/getReal3D/forwardAxis", SetForwardAxis},
                {"/getReal3D/wandLookButtonDown", SetWandLookButtonDown},
                {"/getReal3D/wandLookButtonUp", SetWandLookButtonUp},
                {"/getReal3D/wandDriveButtonDown", SetWandDriveButtonDown},
                {"/getReal3D/wandDriveButtonUp", SetWandDriveButtonUp},
                {"/getReal3D/navSpeedButtonDown", SetNavSpeedButtonDown},
                {"/getReal3D/navSpeedButtonUp", SetNavSpeedButtonUp},
                {"/getReal3D/jumpButtonDown", SetJumpButtonDown},
                {"/getReal3D/jumpButtonUp", SetJumpButtonUp},
                {"/getReal3D/wandButtonDown", SetWandButtonDown},
                {"/getReal3D/wandButtonUp", SetWandButtonUp},
                {"/getReal3D/changeWandButtonDown", SetChangeWandButtonDown},
                {"/getReal3D/changeWandButtonUp", SetChangeWandButtonUp},
                {"/getReal3D/resetButtonDown", SetResetButtonDown},
                {"/getReal3D/resetButtonUp", SetResetButtonUp}
            };

            MessageClient.Instance.AddSubscriptions(subscriptions);
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
            ResetValues();
            yawAxis = StringToFloat(message);
        }

        private void SetPitchAxis(string message)
        {
            ResetValues();
            pitchAxis = StringToFloat(message);
        }

        private void SetStrafeAxis(string message)
        {
            ResetValues();
            strafeAxis = StringToFloat(message);
        }
    
        private void SetForwardAxis(string message)
        {
            ResetValues();
            forwardAxis = StringToFloat(message);
        }

        private void SetWandLookButtonDown(string message)
        {
            ResetValues();
            wandLookButtonDown = true;
            wandLookButtonUp = false;
            wandLookButton = wandLookButtonDown;
        }

        private void SetWandLookButtonUp(string message)
        {
            ResetValues();
            wandLookButtonDown = false;
            wandLookButtonUp = true;
            wandLookButton = false;
        }

        private void SetWandDriveButtonDown(string message)
        {
            ResetValues();
            wandDriveButtonDown = true;
            wandDriveButtonUp = false;
            wandDriveButton = false;
        }

        private void SetWandDriveButtonUp(string message)
        {
            ResetValues();
            wandDriveButtonDown = false;
            wandDriveButtonUp = true;
            wandDriveButton = false;
        }

        private void SetNavSpeedButtonDown(string message)
        {
            ResetValues();
            navSpeedButtonDown = true;
            navSpeedButtonUp = false;
            navSpeedButton = navSpeedButtonDown;
        }

        private void SetNavSpeedButtonUp(string message)
        {
            ResetValues();
            navSpeedButtonDown = false;
            navSpeedButtonUp = true;
            navSpeedButton = false;
        }

        private void SetJumpButtonDown(string message)
        {
            ResetValues();
            jumpButtonDown = true;
            jumpButtonUp = false;
            jumpButton = jumpButtonDown;
        }

        private void SetJumpButtonUp(string message)
        {
            ResetValues();
            jumpButtonDown = false;
            jumpButtonUp = true;
            jumpButton = false;
        }

        private void SetWandButtonDown(string message)
        {
            ResetValues();
            wandButtonDown = true;
            wandButtonUp = false;
            wandButton = wandButtonDown;
        }

        private void SetWandButtonUp(string message)
        {
            ResetValues();
            wandButtonDown = false;
            wandButtonUp = true;
            wandButton = false;
        }

        private void SetChangeWandButtonDown(string message)
        {
            ResetValues();
            changeWandButtonDown = true;
            changeWandButtonUp = false;
            changeWandButton = changeWandButtonDown;
        }

        private void SetChangeWandButtonUp(string message)
        {
            ResetValues();
            changeWandButtonDown = false;
            changeWandButtonUp = true;
            changeWandButton = false;
        }

        private void SetResetButtonDown(string message)
        {
            ResetValues();
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
