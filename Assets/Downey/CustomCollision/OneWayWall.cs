using System;
using Unity.Collections;
using UnityEngine;
using UnityUtils;

namespace Downey.CustomCollision
{
    public class OneWayWall : MonoBehaviour
    {
        public Vector3 passDirection = Vector3.forward;
        public Vector3 spawnPosition = new(0, 1f, -5f);
        
        public GameObject ballPrefab;

        private void Awake()
        {
            passDirection = passDirection.normalized;

            var ball = Instantiate(ballPrefab, transform);
            ball.transform.position = spawnPosition;
            ball.GetOrAdd<Rigidbody>().linearVelocity = Vector3.forward * 30;
            ball.GetOrAdd<SphereCollider>().hasModifiableContacts = true;

            Physics.ContactModifyEvent += OneWayFilter;
        }

        private void OnDestroy()
        {
            Physics.ContactModifyEvent -= OneWayFilter;
        }

        private void OneWayFilter(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
        {
            foreach (var pair in pairs)
            {
                for (int i = 0; i < pair.contactCount; i++)
                {
                    Vector3 normal = pair.GetNormal(i);
                    
                    //If dot(normal,passDirection) > 0,object is approaching from "allowed" side
                    if (Vector3.Dot(normal, passDirection) > 0)
                    {
                        pair.IgnoreContact(i);
                    }
                }
            }
        }
    }
}