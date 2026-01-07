using System;
using UnityEngine;

namespace Downey.Platformer
{
    public class PlatformCollisionHandler : MonoBehaviour
    {
        private Transform platform;

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                // If the contact normal is pointing up,we've collided with the top of the platform
                ContactPoint contactPoint = collision.contacts[0];
                if(contactPoint.normal.y < 0.5f)return;
                
                platform = collision.transform;
                transform.SetParent(platform);
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(null);
                platform = null;
            }
        }
    }
}