using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityUtils;

namespace Downey.DynamicNavMesh
{
    [DefaultExecutionOrder(-115)] // Before input system
    public class DynamicNavMeshUpdater : MonoBehaviour
    {
        [Header("Agent Tracking")]
        [SerializeField, Tooltip("The agent whose position will be used to update the NavMesh")]
        NavMeshAgent trackedAgent;
        
        [SerializeField, Range(0.01f,1f), Tooltip("Quantization factor for position updates (lower = more frequent updates)")]
        float quantizationFactor = 0.1f;

        NavMeshSurface surface;
        private Vector3 volumeSize;
        private void Awake()
        {
            //Debug.Log(new Vector3(1.3f, 0f, 2.7f).Quantize(Vector3.one)); // Expected: (1,0,2)
            //Debug.Log(new Vector3(1.3f, 0f, 2.7f).Quantize(new Vector3(0.5f,0.5f,0.5f))); // Expected: (1,0,2.5)
            surface = GetComponent<NavMeshSurface>();
        }

        private void OnEnable()
        {
            volumeSize = surface.size;
            surface.center = GetQuantizedCenter();
            surface.BuildNavMesh();
        }

        void Update()
        {
            var updatedCenter = GetQuantizedCenter();
            var updateNavMesh = false;

            if (surface.center != updatedCenter)
            {
                surface.center = updatedCenter;
                updateNavMesh = true;
            }

            if (surface.size != volumeSize)
            {
                volumeSize = surface.size;
                updateNavMesh = true;
            }

            if (updateNavMesh)
            {
                surface.UpdateNavMesh(surface.navMeshData);
            }
        }

        Vector3 GetQuantizedCenter()
        {
            return trackedAgent.transform.position.Quantize(quantizationFactor * surface.size);
        }
        
    }
}