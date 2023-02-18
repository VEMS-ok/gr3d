using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ubc.ok.VEMS.gr3d
{
    /// <summary>
    /// This scripts allows the player to cycle through the different wands.
    /// </summary>
    [RequireComponent(typeof(PlayerInputs))]
    public class CustomWandManager: GenericWandManager
    {
        [Tooltip("Events to trigger when wand button is pressed down. Passes the active wand object.")]
        public WandEvent onWandButtonPressed = new WandEvent();

        [Tooltip("Events to trigger when wand button is released. Passes the active wand object.")]
        public WandEvent onWandButtonReleased = new WandEvent();

        private int m_currentIndex = 0;
        private Transform m_currentWand;
        private PlayerInputs m_playerInputs;
        private Interactable m_currentInteractable;
        void Awake()
        {
            m_playerInputs = GetComponent<PlayerInputs>();
            onWandButtonPressed.AddListener(OnWandButtonPressedBehaviour);
            onWandButtonReleased.AddListener(OnWandButtonReleasedBehaviour);
        }

        void Start()
        {
            SetWandActive(0);
        }

        void Update()
        {
            if(m_playerInputs.ChangeWandButtonDown) {
                SetWandActive((m_currentIndex + 1) % m_managedObjects.Count);
            }

            if(m_playerInputs.WandButtonDown) {
                onWandButtonPressed.Invoke(m_managedObjects[m_currentIndex]);
            }
            else if (m_playerInputs.WandButtonUp) {
                onWandButtonReleased.Invoke(m_managedObjects[m_currentIndex]);
            }

            PointerInteraction();
        }

        private void SetWandActive(int index)
        {
            getReal3D.Plugin.debug(string.Format("{0} SetWandActive({1})", gameObject.name, index));
            m_currentIndex = index;
            for(int i = 0; i < m_managedObjects.Count; ++i) {
                bool _isActive = i == index;
                m_managedObjects[i].SetActive(_isActive);
                if (_isActive)
                {
                    m_currentWand = m_managedObjects[i].transform;
                }
            }
        }

        private void OnWandButtonPressedBehaviour(GameObject activeWand) {
            activeWand.GetComponent<ColorBehaviour>()?.EnableColor();
        }

        private void OnWandButtonReleasedBehaviour(GameObject activeWand) {
            activeWand.GetComponent<ColorBehaviour>()?.DisableColor();
        }

        public void PointerInteraction()
        {
            RaycastHit hit;
            if (m_currentWand != null)
            {
                bool hasHit = Physics.Raycast(m_currentWand.position, m_currentWand.up, out hit, LayerMask.NameToLayer("Interactable"));

                Interactable interactable = null;
                if (hasHit)
                {
                    interactable = hit.transform.GetComponent<Interactable>();
                }

                if (m_currentInteractable != interactable)
                {
                    m_currentInteractable?.HoverExit();
                    m_currentInteractable = interactable;
                    interactable?.HoverEnter();
                }

                if (m_currentInteractable != null)
                {
                    if (m_playerInputs.WandButtonDown)
                    {
                        m_currentInteractable.SelectionEnter();
                    }
                    if (m_playerInputs.WandButtonUp)
                    {
                        m_currentInteractable.SelectionExit();
                    }
                }
            }
        }
    }

    [Serializable]
    public class WandEvent: UnityEvent<GameObject>
    {}
}
