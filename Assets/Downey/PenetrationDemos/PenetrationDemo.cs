using System;
using PrimeTween;
using UnityEngine;
using UnityUtils;

namespace Downey.PenetrationDemos
{
    public class PenetrationDemo : MonoBehaviour
    {
        #region Fields
        public LayerMask obstacleLayers;
        //public Color mtvColor = Color.yellow;
        public bool autoResolve = true;
        public bool smoothResolve = true;

        public event Action<Vector3> OnPenetrationStart;
        public event Action<Vector3> OnPenetrationStay;
        public event Action OnPenetrationEnd;

        private Collider col;
        Vector3 lastCorrection;
        bool resolvingCollision;
        #endregion

        void Start()
        {
            col = GetComponent<Collider>();
            OnPenetrationStart += correction =>
            {
                float penetrationDepth = correction.magnitude;
                Debug.Log($"Started penetration, MTV = {penetrationDepth:F3}");

                Tween.ShakeCamera(
                    Camera.main,
                    strengthFactor: penetrationDepth * 2f,
                    duration: 0.3f,
                    frequency: 10);
            };
            OnPenetrationEnd += () => { Debug.Log("Penetration resolved."); };
        }

        void Update()
        {
            bool colliding = col.GetPenetrationsInLayer(obstacleLayers, out Vector3 correction);
            
            //Add small amount to the correction to avoid very close to zero
            correction += correction.normalized * 0.001f;
            lastCorrection = colliding ? correction : Vector3.zero;

            if (colliding)
            {
                if(!resolvingCollision) OnPenetrationStart?.Invoke(correction);
                else OnPenetrationStay?.Invoke(correction);

                resolvingCollision = true;

                if (autoResolve)
                {
                    Vector3 detla = smoothResolve
                        ? Vector3.Lerp(Vector3.zero, correction, 0.05f)
                        : correction;;
                    transform.position += detla;
                }
                Debug.Log($"Corrected collision, MTV = {correction.magnitude:F3}");
            }
            else
            {
                if(resolvingCollision)OnPenetrationEnd?.Invoke();
                resolvingCollision = false;
            }
        }

        // private void OnDrawGizmos()
        // {
        //     if(col == null) col = GetComponent<Collider>();
        //     if(col ==null)return;
        //
        //     if (lastCorrection != Vector3.zero)
        //     {
        //         Vector3 start = col.bounds.center;
        //         Vector3 end = start + lastCorrection;
        //         Gizmos.color = mtvColor;
        //         Gizmos.DrawLine(start, end);
        //         Gizmos.DrawSphere(start, 0.05f);
        //     }
        // }
    }
}