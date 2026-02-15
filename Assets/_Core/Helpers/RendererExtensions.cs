using UnityEngine;

namespace VTLTools
{
    public static class RendererExtensions
    {
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");
        private static readonly int EmissionSelfGlowId = Shader.PropertyToID("_EmissionSelfGlow");
        // Cache static 
        private static MaterialPropertyBlock _mpb;
        private static MaterialPropertyBlock Mpb
        {
            get
            {
                if (_mpb == null) _mpb = new MaterialPropertyBlock();
                return _mpb;
            }
        }

        /// <summary>
        /// eng : Set color using MaterialPropertyBlock (MPB) to avoid creating new material instances.
        /// </summary>
        public static void SetColorMPB(this Renderer renderer, string propertyName, Color color)
        {
            SetColorMPB(renderer, Shader.PropertyToID(propertyName), color);
        }

        /// <summary>
        /// eng : Set color using MaterialPropertyBlock (MPB) to avoid creating new material instances.
        /// </summary>
        public static void SetColorMPB(this Renderer renderer, int propertyId, Color color)
        {
            if (renderer == null) return;

            renderer.GetPropertyBlock(Mpb);

            Mpb.SetColor(propertyId, color);

            renderer.SetPropertyBlock(Mpb);
        }

        /// <summary>
        /// eng : Set emission color with intensity using MaterialPropertyBlock (MPB).
        /// </summary>
        public static void SetEmissionColor(this Renderer renderer, Color color, float intensity)
        {
            int emissionId = EmissionColorId;

            Color hdrColor = color * Mathf.Pow(2, intensity);

            renderer.SetColorMPB(emissionId, hdrColor);
        }

        /// <summary>
        /// eng : Set float value for Emission Self Glow property.
        /// </summary>
        public static void SetEmissionSelfGlow(this Renderer renderer, float value)
        {
            if (renderer == null) return;

            int emissionSelfGlowId = EmissionSelfGlowId;
            renderer.GetPropertyBlock(Mpb);
            Mpb.SetFloat(emissionSelfGlowId, value);
            renderer.SetPropertyBlock(Mpb);
        }
    }
}