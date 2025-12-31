using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtils;

namespace Downey.CullingGroups
{
    public class CullingManager : Singleton<CullingManager>
    {
        #region Fields
        public Camera cullingCamera;
        public float maxCullingDistance = 100f;
        public LayerMask cullableLayers;
        public List<string> cullableTags = new List<string>();
        public float updateInterval = 0.1f;

        private CullingGroup group;
        BoundingSphere[] spheres = new BoundingSphere[64];
        List<CullingTarget> owners = new List<CullingTarget>(64);
        Dictionary<CullingTarget,int> map = new Dictionary<CullingTarget,int>(64);
        private int count;
        private float tPos;
        int[] tmp = new int[256];
        private HashSet<string> tagSet;
        #endregion

        new void Awake()
        {
            base.Awake();
            
            if(!cullingCamera) cullingCamera = Camera.main;

            group = new CullingGroup();
            group.onStateChanged = OnStateChanged;
            group.targetCamera = cullingCamera;
            group.SetBoundingSpheres(spheres);
            group.SetBoundingSphereCount(0);
            group.SetDistanceReferencePoint(cullingCamera.transform);
            group.SetBoundingDistances(new float[]{maxCullingDistance}); // single band
            //group.SetBoundingDistances(new float[]{ 10f, 25f, 43f }); //example of multiple bands,increasing distance
            
            tagSet = new HashSet<string>(cullableTags);
        }

        void Update()
        {
            tPos += Time.deltaTime;

            if (tPos >= updateInterval)
            {
                for (int i = 0; i < count; i++)
                {
                    var o = owners[i];
                    if(!o) continue;
                    
                    var s = spheres[i];
                    s.position = o.transform.position;
                    s.radius = 1f; // TODO:radius
                    spheres[i] = s;
                }
                tPos = 0;
            }
        }
        
        public void Register(CullingTarget t)
        {
            if(!t) return;

            if (count == spheres.Length)
            {
                System.Array.Resize(ref spheres, count * 2);
                group.SetBoundingSpheres(spheres);
            }
            
            owners.Add(t);
            map[t] = count;
            spheres[count] = new BoundingSphere(t.transform.position, 1f); //TODO: radius
            count++;
            group.SetBoundingSphereCount(count);
        }

        public void Unregister(CullingTarget t)
        {
            if(group == null || !t || !map.TryGetValue(t, out int i)) return;
            
            group.EraseSwapBack(i);
            CullingGroup.EraseSwapBack(i,spheres,ref count);
            var last = owners.Count - 1;
            var moved = owners[last];
            owners[i] = moved;
            owners.RemoveAt(last);
            if (moved) map[moved] = i;
            map.Remove(t);
            group.SetBoundingSphereCount(count);
        }

        private void OnStateChanged(CullingGroupEvent e)
        {
            var cullingTarget = owners[e.index];
            if(!cullingTarget) return;

            if (!IsCullable(cullingTarget.gameObject))
            {
                cullingTarget.ToogleOn();
                return;
            }

            bool isRange = e.currentDistance == 0;
            if(e.isVisible && isRange) cullingTarget.ToogleOn();
            else cullingTarget.ToogleOff();
            
        }

        bool IsCullable(GameObject obj)
        {
            return ((1 << obj.layer) & cullableLayers) != 0 && tagSet.Contains(obj.tag); // layer and tag check
        }

        private void OnDisable()
        {
            if (group != null)
            {
                group.onStateChanged = null;
                group.Dispose();
                group = null;
            }
        }
    }
}