using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

namespace Downey.DecalDemo
{
    public class BulletHoleSpawner : MonoBehaviour
    {
        #region Fields
        private IObjectPool<DecalProjector> decalPool;
        [Tooltip("Material to use for the bullet hole decal.")]
        public Material decalMaterial;
        [Tooltip("Layers that can receive bullet hole decals.")]
        public LayerMask decalLayers = -1;
        [Tooltip("Size of each decal in world uints (x = width, y = height, z = depth).")]
        public Vector3 decalSize = new Vector3(0.5f, 0.5f, 0.5f);
        [Tooltip("Duration in seconds for the decal to fade out before being returned to the pool.")]
        public float fadeDuration = 5f;

        private Camera cam;

        #endregion

        void Start()
        {
            cam = Camera.main;

            decalPool = new ObjectPool<DecalProjector>(
                createFunc: () =>
                {
                    GameObject go = new GameObject("Decal Projector");
                    go.transform.SetParent(transform);
                    DecalProjector dp = go.AddComponent<DecalProjector>();
                    dp.material = decalMaterial;
                    dp.fadeFactor = 1f;
                    dp.fadeScale = 0.95f;
                    dp.startAngleFade = 0f;
                    dp.endAngleFade = 30f;
                    return dp;
                },
                actionOnGet: dp => dp.gameObject.SetActive(true),
                actionOnRelease: dp => dp.gameObject.SetActive(false),
                actionOnDestroy: dp => Destroy(dp.gameObject),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 20
            );
        }

        void SpawnDecal(RaycastHit hit)
        {
            DecalProjector projector = decalPool.Get();
            
            projector.transform.position = hit.point + hit.normal * 0.01f;
            
            Quaternion normalRotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
            Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
            projector.transform.rotation = normalRotation*randomRotation;

            projector.size = decalSize;
            StartCoroutine(FadeAndRelease(projector,fadeDuration));
        }
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.SphereCast(ray, decalSize.x * 0.5f, out RaycastHit hit, Mathf.Infinity, decalLayers))
                {
                    SpawnDecal(hit);
                }
            }
        }
        
        IEnumerator FadeAndRelease(DecalProjector projector, float duration)
        {
            float time = 0f;
            float initialFade = projector.fadeFactor;

            while (time < duration)
            {
                if(projector == null)yield break;
                
                time += Time.deltaTime;
                float t = time / duration;
                projector.fadeFactor = Mathf.Lerp(initialFade, 0f, t);
                yield return null;
            }

            if (projector != null)
            {
                projector.fadeFactor = initialFade;
                decalPool.Release(projector);
            }
        }
    }
    
}