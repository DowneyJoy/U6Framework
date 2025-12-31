using System;
using Downey.ImprovedTimers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Downey.CullingGroups
{
    public enum CullingBehaviour { None, ToggleScripts, FadeInOut }
    public class CullingTarget : MonoBehaviour
    {
        #region Fields
        public UnityEvent onCulled,onVisible;
        public float boundarySphereRadius = 1f;
        public Renderer objectRenderer;
        public float fadeDuration = 2f;
        public CullingBehaviour cullingMode = CullingBehaviour.FadeInOut;
        public bool isPriorityObject;

        private MaterialPropertyBlock mpb;
        private MonoBehaviour[] scripts;
        
        static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        static readonly int ColorId = Shader.PropertyToID("_Color");
        
        CountdownTimer fadeTimer;
        private float startAlpha;
        float currentAlpha;
        float targetAlpha;

        private NavMeshAgent agent;
        #endregion

        void Awake()
        {
            objectRenderer = gameObject.GetComponentInChildren<Renderer>();
            scripts = GetComponents<MonoBehaviour>();
            agent = GetComponent<NavMeshAgent>();

            for (int i = 0; i < scripts.Length; i++)
            {
                if(scripts[i] == this) scripts[i] = null;
            }
            
            fadeTimer = new CountdownTimer(fadeDuration);
        }

        private void OnEnable()
        {
            currentAlpha = GetAlpha();
            
            if(isPriorityObject) onVisible?.Invoke();
            else CullingManager.Instance.Register(this);
        }

        void OnDisable()
        {
            if(!isPriorityObject) CullingManager.Instance.Unregister(this);
        }

        void Update()
        {
            if (fadeTimer.IsRunning)
            {
                float t = 1f - Mathf.Clamp01(fadeTimer.Progress);
                float a = Mathf.Lerp(startAlpha, targetAlpha, t);
                SetAlpha(a);
            }
        }
        float GetAlpha()
        {
            if (!objectRenderer) return 1f;

            var m = objectRenderer.sharedMaterial;
            if (!m) return 1f;
            
            if(m.HasProperty(BaseColorId)) return m.GetColor(BaseColorId).a;
            if(m.HasProperty(ColorId)) return m.GetColor(ColorId).a;

            return 1f;
        }

        void SetAlpha(float a)
        {
            if(!objectRenderer) return;
            
            var m = objectRenderer.sharedMaterial;
            if(!m) return;
            
            currentAlpha = Mathf.Clamp01(a);
            
            mpb ??= new MaterialPropertyBlock();
            objectRenderer.GetPropertyBlock(mpb);

            if (m.HasProperty(BaseColorId))
            {
                var c =  m.GetColor(BaseColorId);
                c.a = currentAlpha;
                mpb.SetColor(BaseColorId, c);
            }
            else if (m.HasProperty(ColorId))
            {
                var c = m.GetColor(ColorId);
                c.a = currentAlpha;
                mpb.SetColor(ColorId, c);
            }
            
            objectRenderer.SetPropertyBlock(mpb);
        }

        void BeginFadeTo(float target, bool deactivate)
        {
            if(!objectRenderer) return;
            mpb ??=  new MaterialPropertyBlock();
            
            startAlpha = currentAlpha;
            targetAlpha = Mathf.Clamp01(target);

            if (deactivate && targetAlpha <= 0f)
            {
                fadeTimer.OnTimerStop = () =>
                {
                    if (objectRenderer) objectRenderer.enabled = false;
                };
            }
            else
            {
                fadeTimer.OnTimerStop = () => { };
            }
            fadeTimer.Reset(fadeDuration);
            fadeTimer.Start();
        }

        void EnableSripts(bool v)
        {
            for (int i = 0; i < scripts.Length; i++)
            {
                var s = scripts[i];
                if(s == null) continue;
                s.enabled = v;
            }
            if(agent) agent.enabled = v;
        }

        public void ToogleOn()
        {
            if(fadeTimer.IsRunning) fadeTimer.Stop();

            if (isPriorityObject)
            {
                onVisible?.Invoke();
                return;
            }

            switch (cullingMode)
            {
                case CullingBehaviour.FadeInOut:
                    if(objectRenderer && !objectRenderer.enabled) objectRenderer.enabled = true;
                    BeginFadeTo(1f, deactivate: false);
                    break;
                case CullingBehaviour.ToggleScripts:
                    EnableSripts(true);
                    break;
            }
            onVisible?.Invoke();
        }

        public void ToogleOff()
        {
            if(fadeTimer.IsRunning) fadeTimer.Stop();
            if(isPriorityObject) return;

            switch (cullingMode)
            {
                case CullingBehaviour.FadeInOut:
                    BeginFadeTo(0f,deactivate:true);
                    break;
                case CullingBehaviour.ToggleScripts:
                    EnableSripts(false);
                    break;
            }
            
            onCulled?.Invoke();
        }
    }
}