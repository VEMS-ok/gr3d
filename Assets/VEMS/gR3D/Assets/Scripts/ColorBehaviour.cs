using UnityEngine;

namespace ubc.ok.VEMS.gr3d
{
    /// <summary>
    /// Allows modifying the color with a switch.
    /// </summary>
    public class ColorBehaviour : MonoBehaviour
    {
        [Tooltip("The color to set when enabled.")]
        /// <summary>
        /// The color to set when enabled.
        /// </summary>
        public Color color = Color.yellow;
        [Tooltip("The meshrender whose color will be toggled. If not set will look for a MeshRenderer in the attached GameObject")]
        /// <summary>
        /// The meshrender whose color will be toggled. If not set will look for a MeshRenderer in the attached GameObject
        /// </summary>
        public MeshRenderer meshRenderer;

        private Color defaultColor;
        private bool colorEnabled = false;

        void Awake()
        {
            if (meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
        }

        /// <summary>
        /// Set the color configured. If EnableColor had already been called and DisableColor has not been called yet,
        /// calling this will not do anything.
        /// </summary>
        public void EnableColor()
        {
            if (!colorEnabled)
            {
                defaultColor = meshRenderer.material.color;
                meshRenderer.material.color = color;
                colorEnabled = true;
            }
        }

        /// <summary>
        /// Restores the color of the render before `EnableColor` was called. If EnableColor has not been called,
        /// this does nothing.
        /// </summary>
        public void DisableColor()
        {
            if (colorEnabled)
            {
                meshRenderer.material.color = defaultColor;
                colorEnabled = false;
            }
        }
    }
}
