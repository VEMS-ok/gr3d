﻿using System;
using UnityEngine;
using UnityEngine.Events;

namespace ubc.ok.VEMS.gr3d
{
    /// <summary>
    /// This script handles grabbing objects with a wand. Extend the GenericGrabbingWand with UnityEvents.
    /// </summary>
    [RequireComponent(typeof(PlayerInputs))]
    public class CustomGrabbingWand: GenericGrabbingWand
    {
        private GameObject m_grabObject = null;
        private PlayerInputs m_playerInputs;

        [Tooltip("Eventcallback when object was grabbed.")]
        public GrabbableObjectEvent onGrabbed = new GrabbableObjectEvent();

        [Tooltip("Eventcallback when grabbed object is dropped.")]
        public GrabbableObjectEvent onDropped = new GrabbableObjectEvent();

        void Awake()
        {
            m_playerInputs = GetComponent<PlayerInputs>();
        }

        void OnDisable()
        {
            DropObject();
        }

        void DropObject()
        {
            if(m_grabObject != null) {
                // If the object has the GrabbedObject behavior, tell it to drop
                GrabbedObject grabbedObject = m_grabObject.GetComponent<GrabbedObject>();
                if (grabbedObject)
                {
                    grabbedObject.dropObject(ReparentTransform);
                    onDropped.Invoke(m_grabObject);
                }

                m_grabObject = null;
            }
        }

        void Update()
        {
            if(!Wand || !Wand.gameObject.activeInHierarchy) {
                return;
            }

            Debug.DrawRay(Wand.parent.position, Wand.parent.forward * 2f, Color.yellow);

            // If the wand button is released, drop the object
            if(m_playerInputs.WandButtonUp) {
                DropObject();
            }
            // If the wand button was pressed and we're not already grabbing something, test for objects to grab
            else if(m_grabObject == null && m_playerInputs.WandButtonDown) {
                // Raycast test for objects to grab
                RaycastHit hit = new RaycastHit();
                bool hitTest = Physics.Raycast(Wand.parent.position, Wand.parent.forward, out hit, 2.0f, grabLayerMask);
                if(hitTest) {
                    Rigidbody rb = hit.rigidbody;
                    Transform tf = hit.transform.parent;
                    while(rb == null && tf && tf.parent != null) {
                        tf = tf.parent;
                        rb = tf.GetComponent<Rigidbody>();
                    }

                    // If the object doesn't have a rigidbody, don't do anything
                    if(!rb)
                        return;

                    m_grabObject = rb.gameObject;

                    // Add the GrabbedObject behavior(script) to the object if it hasn't already been grabbed by someone else
                    GrabbedObject grabbedObject = m_grabObject.GetComponent<GrabbedObject>();
                    if(!grabbedObject) {
                        grabbedObject = m_grabObject.AddComponent<GrabbedObject>();
                    }

                    // Grab the object
                    grabbedObject.grabObject(ReparentTransform, allowGrabSteal);
                    onGrabbed.Invoke(m_grabObject);
                }
            }
        }
    }

    [Serializable]
    public class GrabbableObjectEvent: UnityEvent<GameObject>
    {}
}
