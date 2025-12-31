using System;
using System.Linq;
using Downey.Damageable;
using PrimeTweenDemo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityUtils;

namespace Downey.TargetingStrategy
{
    [Serializable]
    public class AOETargeting : TargetingStrategy
    {
        public GameObject aoePrefab;
        public float aoeRadius = 5f;
        public LayerMask groundLayerMask;

        private GameObject previewInstance;

        public override void Start(Ability ability, TargetingManager targetingManager)
        {
            this.ability = ability;
            this.targetingManager = targetingManager;
            isTargeting = true;
            
            targetingManager.SetTargetingStrategy(this);
            
            if(aoePrefab != null)
                previewInstance = UnityEngine.Object.Instantiate<GameObject>(aoePrefab, Vector3.zero.Add(y:0.1f), Quaternion.identity);
            
            if(targetingManager.input != null)
                targetingManager.input.Click += OnClick;
        }

        public override void Update()
        {
            if(!isTargeting || previewInstance == null) return;

            previewInstance.transform.position = GetMouseWorldPosition().Add(y: 0.1f);
        }

        Vector3 GetMouseWorldPosition()
        {
            if(targetingManager.cam == null) return Vector3.zero;
            
            var ray = targetingManager.cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            return Physics.Raycast(ray, out var hit,100f,groundLayerMask)? hit.point : Vector3.zero;
        }

        public override void Cancel()
        {
            isTargeting = false;
            
            targetingManager.ClearTargetingStrategy();
            if (previewInstance != null)
                UnityEngine.Object.Destroy(previewInstance);
            if(targetingManager.input != null)
                targetingManager.input.Click -= OnClick;
        }

        void OnClick(RaycastHit hit)
        {
            if (isTargeting)
            {
                var targets = Physics.OverlapSphere(hit.point, aoeRadius)
                    .Select(c => c.GetComponent<IDamageable>())
                    .OfType<IDamageable>();
                
                foreach (var target in targets)
                    ability.Execute(target);
                
                Cancel();
            }
        }
    }
}