using UnityEngine;
using UnityEngine.Events;

namespace ubc.ok.VEMS.gr3d
{
    public class Interactable : MonoBehaviour
    {
        [Tooltip("Event fired while a wand is hovering on the object. This event will be continuously called.")]
        public InteractableEvent OnHover;
        [Tooltip("Event fired while a wand hovers over this object.")]
        public InteractableEvent OnHoverEnter;
        [Tooltip("Event fired while a wand stops hovering.")]
        public InteractableEvent OnHoverExit;

        [Tooltip("Event fired when the object is selected. This event will be continuosly called while in selection.")]
        public InteractableEvent OnSelection;
        [Tooltip("Event fired when the object is selected.")]
        public InteractableEvent OnSelectionStart;
        [Tooltip("Event fired when the object seleciton exited.")]
        public InteractableEvent OnSelectionEnd;

        private bool hovering = false;
        private bool selected = false;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (hovering) 
            {
                OnHover.Invoke(gameObject);
            }

            if (selected)
            {
                OnSelection.Invoke(gameObject);
            }
        }

        public void HoverEnter()
        {
            if (!hovering)
            {
                OnHoverEnter.Invoke(gameObject);
            }
            hovering = true;
        }

        public void HoverExit()
        {
            if (hovering)
            {
                OnHoverExit.Invoke(gameObject);
            }
            hovering = false;
        }

        public void SelectionEnter()
        {
            if (!selected)
            {
                OnSelectionStart.Invoke(gameObject);
            }
            selected = true;
        }

        public void SelectionExit()
        {
            if (selected)
            {
                OnSelectionEnd.Invoke(gameObject);
            }
            selected = false;
        }
    }

    public class InteractableEvent: UnityEvent<GameObject>
    {}
}
