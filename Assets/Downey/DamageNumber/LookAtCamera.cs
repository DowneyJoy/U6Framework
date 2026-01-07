using System;
using UnityEngine;

namespace Downey.DamageNumber
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]Camera targetCamera;

        private void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = Camera.main;
            }
        }

        void Update()
        {
            if (targetCamera != null)
            {
                transform.LookAt(targetCamera.transform);
                transform.Rotate(0,180,0);
            }
        }
    }
}