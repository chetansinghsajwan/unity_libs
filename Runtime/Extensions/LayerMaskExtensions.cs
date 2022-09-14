using UnityEngine;

namespace GameFramework.Extensions
{
    public static class LayerMaskExtensions
    {
        public static bool Contains(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}