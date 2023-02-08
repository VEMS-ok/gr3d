using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This scripts allows the player to cycle through the different wands.
/// </summary>
[RequireComponent(typeof(PlayerInputs))]
public class CustomWandManager: MonoBehaviour
{
    [Tooltip("List of the different wands the player can choose from.")]
    public List<GameObject> m_managedObjects;

    [Tooltip("Events to trigger when wand button is pressed down. Passes the active wand object.")]
    public WandEvent onWandButtonPressed = new WandEvent();

    [Tooltip("Events to trigger when wand button is released. Passes the active wand object.")]
    public WandEvent onWandButtonReleased = new WandEvent();

    private int m_activeIndex = 0;
    private PlayerInputs m_playerInputs;
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
    }

    private void SetWandActive(int index)
    {
        getReal3D.Plugin.debug(string.Format("{0} SetWandActive({1})", gameObject.name, index));
        m_activeIndex = index;
        for(int i = 0; i < m_managedObjects.Count; ++i) {
            m_managedObjects[i].SetActive(i == index);
        }
    }

    private void OnWandButtonPressedBehaviour(GameObject activeWand) {
        activeWand.GetComponent<ColorBehaviour>()?.EnableColor();
    }

    private void OnWandButtonReleasedBehaviour(GameObject activeWand) {
        activeWand.GetComponent<ColorBehaviour>()?.DisableColor();
    }
}

public class WandEvent: UnityEvent<GameObject>
{}
