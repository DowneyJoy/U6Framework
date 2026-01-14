using UnityEngine;

namespace UnityUtils
{
    public static class ColliderExtensions
    {
        static readonly Collider[] overlapCache = new Collider[32];
        
        //Finds all penetrations in a lager mask and sums their MTVs
        public static bool GetPenetrationsInLayer(this Collider source, LayerMask layerMask,
            out Vector3 totalCorrection)
        {
            totalCorrection = Vector3.zero;
            if(source == null) return false;
            int count = Physics.OverlapBoxNonAlloc(
                source.bounds.center,
                source.bounds.extents,
                overlapCache,
                source.transform.rotation,
                layerMask);
            bool collided = false;
            for (int i = 0; i < count; i++)
            {
                Collider collider = overlapCache[i];
                if(collider == source) continue;
                if (source.ComputePenetration(collider, out Vector3 dir, out float dist))
                {
                    collided = true;
                    totalCorrection += dir * dist;
                }
            }
            return collided;
        }
        // Compute the minimum translation vector (MTV) between two MTVs
        public static bool ComputePenetration(this Collider source, Collider target, out Vector3 direction,
            out float distance)
        {
            direction = Vector3.zero;
            distance = 0;
            if (source == null || target == null) return false;

            return Physics.ComputePenetration(
                source, source.transform.position, source.transform.rotation,
                target, target.transform.position, target.transform.rotation,
                out direction, out distance);
        }
    }
}