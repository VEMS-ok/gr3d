using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using getReal3D;

namespace getReal3D.ubco
{
    [RequireComponent(typeof(PlayerInputs))]
    public class  WandPitchAxis: MonoBehaviour
    {
        [Tooltip("The Wand object")]
        public Transform hand;

        [Tooltip("The rotation speed")]
        public float rotationSpeed = 40;

        private PlayerInputs m_playerInputs;
        private NavigationHelper navigationHelper;

        void Awake()
        {
            m_playerInputs = GetComponent<PlayerInputs>();
            navigationHelper = GetComponent<Navigation>().navigationHelper;
            if (GetComponent<GenericWandUpdater>() != null && GetComponent<GenericWandUpdater>().enabled) {
                Debug.LogError("'GenericWandUpdater' and 'GenericWandPitchAxis' cannot be active at the same time.");
            }
        }


        void Update()
        {
            // inpsired by getReal3D's NavigationHelper's UpdateRotation
            float joyY = m_playerInputs.PitchAxis * rotationSpeed * Time.smoothDeltaTime;

            hand?.Rotate(hand.right, -joyY, Space.World);
        }
    }
}
