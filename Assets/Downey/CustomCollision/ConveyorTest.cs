using System;
using Unity.Collections;
using UnityEngine;
using UnityUtils;

namespace Downey.CustomCollision
{
    public class ConveyorTest : MonoBehaviour
    {
        public Vector3 conveyorDirection = Vector3.right * 5f;
        public Vector3 spawnPosition = new Vector3(0,2f,0);
        public float conveyorZoneRadius = 5f;

        public GameObject ballPrefab;

        private void OnEnable()
        {
            var ball = Instantiate(ballPrefab);
            ball.transform.position = spawnPosition;
            ball.GetOrAdd<SphereCollider>().hasModifiableContacts = true;

            Physics.ContactModifyEvent += ConveyorEffect;
        }

        void OnDisable()
        {
            Physics.ContactModifyEvent -= ConveyorEffect;
        }

        private void ConveyorEffect(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
        {
            foreach (ModifiableContactPair pair in pairs)
            {
                for (int i = 0; i < pair.contactCount; i++)
                {
                    Vector3 contactPoint = pair.GetPoint(i);
                    if (Vector3.Distance(contactPoint, Vector3.zero) < conveyorZoneRadius)
                    {
                        pair.SetTargetVelocity(i, conveyorDirection);
                    }
                }
            }
        }
    }
}