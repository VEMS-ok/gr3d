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

        private int m_activeIndex = 0;
        private Transform m_activeWand;
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
                SetWandActive((m_activeIndex + 1) % m_managedObjects.Count);
            }

            if(m_playerInputs.WandButtonDown) {
                onWandButtonPressed.Invoke(m_managedObjects[m_activeIndex]);
            }
            else if (m_playerInputs.WandButtonUp) {
                onWandButtonReleased.Invoke(m_managedObjects[m_activeIndex]);
            }

            PointerInteraction();
        }

        private void SetWandActive(int index)
        {
            getReal3D.Plugin.debug(string.Format("{0} SetWandActive({1})", gameObject.name, index));
            m_activeIndex = index;
            for(int i = 0; i < m_managedObjects.Count; ++i) {
                bool _isActive = i == index;
                m_managedObjects[i].SetActive(_isActive);
                if (_isActive)
                {
                    m_activeWand = m_managedObjects[i].transform;
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
            if (m_activeWand != null && Physics.Raycast(m_activeWand.position, m_activeWand.up, out hit, 100, LayerMask.NameToLayer("Interactable")))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
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
                    if (m_playerInputs.WandLookButtonUp)
                    {
                        m_currentInteractable.SelectionExit();
                    }
                }
            }
        }
    }

    public class WandEvent: UnityEvent<GameObject>
    {}
}
