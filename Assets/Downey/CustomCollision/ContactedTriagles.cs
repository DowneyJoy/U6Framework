using System;
using System.Collections.Generic;
using System.Linq;
using ConcurrentCollections;
using Unity.Collections;
using UnityEngine;
using UnityUtils;

namespace Downey.CustomCollision
{
    public class ContactedTriagles : MonoBehaviour
    {
        public GameObject terrainPrefab;
        public GameObject ballPrefab;

        private Mesh mesh;
        MeshCollider meshCollider;
        private int colliderId;

        private readonly ConcurrentHashSet<uint> touchedTriangles = new();

        private void Awake()
        {
            var terrain = Instantiate(terrainPrefab);
            terrain.transform.position = Vector3.zero;
            mesh = terrain.GetOrAdd<MeshFilter>().mesh;
            meshCollider = terrain.GetOrAdd<MeshCollider>();
            colliderId = meshCollider.GetInstanceID();
            
            var ball = Instantiate(ballPrefab);
            ball.transform.position = new Vector3(2, 5, 2);
            ball.GetComponent<SphereCollider>().hasModifiableContacts = true;

            Physics.ContactModifyEvent += StoreTouchedTriangles;
        }

        private void OnDestroy()
        {
            Physics.ContactModifyEvent -= StoreTouchedTriangles;
        }

        private void StoreTouchedTriangles(PhysicsScene scene, NativeArray<ModifiableContactPair> pairs)
        {
            foreach (var pair in pairs)
            {
                if (pair.colliderInstanceID != colliderId && pair.otherColliderInstanceID != colliderId)
                {
                    continue;
                }

                for (int i = 0; i < pair.contactCount; i++)
                {
                    uint faceIndex = pair.GetFaceIndex(i);
                    touchedTriangles.Add(faceIndex);
                    pair.IgnoreContact(i);
                }
            }
        }

        void FixedUpdate()
        {
            if(touchedTriangles.IsEmpty)return;
            
            var triangleIndices = new List<uint>(touchedTriangles.ToList());
            touchedTriangles.Clear();

            List<int> updatedTriangles = new List<int>();
            int[] originalTriangles = mesh.triangles;

            for (int i = 0; i < originalTriangles.Length; i+=3)
            {
                int triangleIndex = i / 3;
                if (triangleIndices.Contains((uint)triangleIndex))
                {
                    continue;
                }
                
                updatedTriangles.Add(originalTriangles[i+0]);
                updatedTriangles.Add(originalTriangles[i+1]);
                updatedTriangles.Add(originalTriangles[i+2]);
            }
            
            mesh.triangles = updatedTriangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();

            meshCollider.sharedMesh = null; // Force internal collider update
            meshCollider.sharedMesh = mesh;
        }
    }
}