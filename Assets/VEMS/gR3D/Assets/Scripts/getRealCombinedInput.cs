using System.Collections.Generic;
using UnityEngine;
using getReal3D;

namespace ubc.ok.VEMS.gr3d
{
    /// <summary>
    /// Combine multiple different classes that implement the `PlayerInputs` interface. Hence, this can be used as a dropin
    /// replacement for anywhere the `PlayerInputs` is used. Add the different PlayerInputs classes to `playerInputsComponents`.
    /// Note tha each class that implements the PlayerInput would have to be in its own GameObject for this to work.
    /// Currently, the `Wand` and `Head` would simply return only the values from the first component in `playerInputsComponents`.
    /// </summary>
    public class getRealCombinedInput : getReal3D.MonoBehaviourWithRpc, PlayerInputs
    {
        [Tooltip("Combines the inputs from multiple PlayerInputs. Precedence order is the same as the order in this list (i.e., first one with a default input returns the value.). Each gameObject in the list is expected to have a PlayerInput attached to it.")]
        /// <summary>
        /// Combines the inputs from multiple PlayerInputs. Precedence order is the same as the order in this list (i.e., first one with a default input returns the value.). Each gameObject in the list is expected to have a PlayerInput attached to it.
        /// </summary>
        public List<Transform> playerInputsComponents;

        private List<PlayerInputs> playerInputs;

        private void Awake()
        {
            playerInputs = new List<PlayerInputs>();
            foreach(Component playerInputComponent in playerInputsComponents)
            {
                PlayerInputs playerInput = playerInputComponent.GetComponent<PlayerInputs>();
                if (playerInput == null){
                    throw new UnityException("All player inputs should extend getReal3D.PlayerInputs");
                }
                else
                {
                    playerInputs.Add((PlayerInputs) playerInput);
                }
            }
            Debug.Log($"Registered {playerInputs.Count} PlayerInputs");

        }

        #region PlayerInputs implementation
        public MonoBehaviour behaviour
        {
            get
            {
                return this;
            }
        }

        public float YawAxis
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.YawAxis != 0)
                    {
                        return input.YawAxis;
                    }
                }

                return 0;
            }
        }

        public float PitchAxis
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.PitchAxis != 0)
                    {
                        return input.PitchAxis;
                    }
                }

                return 0;
            }
        }

        public bool WandLookButtonDown
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandLookButtonDown)
                    {
                        return input.WandLookButtonDown;
                    }
                }

                return false;
            }
        }

        public bool WandLookButtonUp
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandLookButtonUp)
                    {
                        return input.WandLookButtonUp;
                    }
                }

                return false;
            }
        }

        public bool WandLookButton
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandLookButton)
                    {
                        return input.WandLookButton;
                    }
                }

                return false;
            }
        }

        public bool WandDriveButtonDown
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandDriveButtonDown)
                    {
                        return input.WandDriveButtonDown;
                    }
                }

                return false;
            }
        }

        public bool WandDriveButtonUp
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandDriveButtonUp)
                    {
                        return input.WandDriveButtonUp;
                    }
                }

                return false;
            }
        }

        public bool WandDriveButton
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandDriveButton)
                    {
                        return input.WandLookButtonDown;
                    }
                }

                return false;
            }
        }

        public float StrafeAxis
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.StrafeAxis != 0)
                    {
                        return input.StrafeAxis;
                    }
                }

                return 0;
            }
        }

        public float ForwardAxis
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.ForwardAxis != 0)
                    {
                        return input.ForwardAxis;
                    }
                }

                return 0;
            }
        }

        public bool NavSpeedButtonDown
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.NavSpeedButtonDown)
                    {
                        return input.NavSpeedButtonDown;
                    }
                }

                return false;
            }
        }

        public bool NavSpeedButtonUp
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.NavSpeedButtonUp)
                    {
                        return input.NavSpeedButtonUp;
                    }
                }

                return false;
            }
        }

        public bool NavSpeedButton
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.NavSpeedButton)
                    {
                        return input.NavSpeedButton;
                    }
                }

                return false;
            }
        }

        public bool JumpButtonDown
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.JumpButtonDown)
                    {
                        return input.JumpButtonDown;
                    }
                }

                return false;
            }
        }

        public bool JumpButtonUp
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.JumpButtonUp)
                    {
                        return input.JumpButtonUp;
                    }
                }

                return false;
            }
        }

        public bool JumpButton
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.JumpButton)
                    {
                        return input.JumpButton;
                    }
                }

                return false;
            }
        }

        public bool WandButtonDown
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandButtonDown)
                    {
                        return input.WandButtonDown;
                    }
                }
                return false;
            }
        }

        public bool WandButtonUp
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandButtonUp)
                    {
                        return input.WandButtonUp;
                    }
                }

                return false;
            }
        }

        public bool WandButton
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.WandButton)
                    {
                        return input.WandButton;
                    }
                }

                return false;
            }
        }

        public bool ChangeWandButtonDown
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.ChangeWandButtonDown)
                    {
                        return input.ChangeWandButtonDown;
                    }
                }

                return false;
            }
        }

        public bool ChangeWandButtonUp
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.ChangeWandButtonUp)
                    {
                        return input.ChangeWandButtonUp;
                    }
                }

                return false;
            }
        }

        public bool ChangeWandButton
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.ChangeWandButton)
                    {
                        return input.ChangeWandButton;
                    }
                }

                return false;
            }
        }

        public bool ResetButtonDown
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.ResetButtonDown)
                    {
                        return input.ResetButtonDown;
                    }
                }

                return false;
            }
        }

        public bool ResetButtonUp
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.ResetButtonUp)
                    {
                        return input.ResetButtonUp;
                    }
                }

                return false;
            }
        }

        public bool ResetButton
        {
            get
            {
                foreach (PlayerInputs input in playerInputs)
                {
                    if (input.ResetButton)
                    {
                        return input.ResetButton;
                    }
                }

                return false;
            }
        }

        public Sensor Wand
        {
            get
            {
                // FIXME: How to handle precedence here?
                return playerInputs[0].Wand;
            }
        }

        public Sensor Head
        {
            get
            {
                // FIXME: How to handle precedence here?
                return playerInputs[0].Head;
            }
        }
        #endregion
    }
}
