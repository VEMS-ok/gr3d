using UnityEngine;

/// <summary>
/// Allows modifying the color with a switch.
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
public class ColorBehaviour : MonoBehaviour
{
    [Tooltip("The color to set when enabled.")]
    public Color color = Color.yellow;

    private Color defaultColor;
    private MeshRenderer meshRenderer;

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
    }

    public void EnableColor() {
        meshRenderer.material.color = color;
    }

    public void DisableColor() {
        meshRenderer.material.color = defaultColor;
    }
}
