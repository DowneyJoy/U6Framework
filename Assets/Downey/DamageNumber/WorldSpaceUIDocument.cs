using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityUtils;

namespace Downey.DamageNumber
{
    public class WorldSpaceUIDocument : MonoBehaviour
    {
        #region fields
        const string k_transparentShader = "Unlit/Transparent";
        const string k_textureShader = "Unlit/Texture";
        const string k_mainTex = "_MainTex";
        static readonly int MainTex = Shader.PropertyToID(k_mainTex);

        [SerializeField] private int panelWidth = 1280;
        [SerializeField] private int panelHeight = 720;
        [SerializeField]float panelScale = 1f;
        [SerializeField]float pixelsPerUnit = 500f;
        [SerializeField] private VisualTreeAsset visualTreeAsset;
        [SerializeField] PanelSettings panelSettingsAsset;
        [SerializeField]RenderTexture renderTextureAsset;
        
        MeshRenderer meshRenderer;
        UIDocument uiDocument;
        PanelSettings panelSettings;
        RenderTexture renderTexture;
        Material material;

        #endregion

        public void SetLabelText(string label, string text)
        {
            if (uiDocument.rootVisualElement == null)
            {
                uiDocument.visualTreeAsset = visualTreeAsset;
            }
            
            //Consider caching the label element for better performance
            uiDocument.rootVisualElement.Q<Label>(label).text = text;
        }

        private void Awake()
        {
            InitializeComponents();
            BuildPanel();
        }

        void BuildPanel()
        {
            CreateRenderTexture();
            CreatePanelSettings();
            CreateUIDocument();
            CreateMaterial();
            
            SetMaterialToRenderer();
            SetPanelSize();
        }

        void SetMaterialToRenderer()
        {
            if(meshRenderer != null)
                meshRenderer.sharedMaterial = material;
        }
        
        void SetPanelSize()
        {
            if (renderTexture != null && (renderTexture.width != panelWidth || renderTexture.height != panelHeight))
            {
                renderTexture.Release();
                renderTexture.width = panelWidth;
                renderTexture.height = panelHeight;
                renderTexture.Create();
                
                uiDocument?.rootVisualElement?.MarkDirtyRepaint();
            }
            transform.localScale = new Vector3(panelWidth/pixelsPerUnit, panelHeight/pixelsPerUnit, 1);
        }

        void CreateMaterial()
        {
            string shaderName = panelSettings.colorClearValue.a < 1.0f ? k_transparentShader : k_textureShader;
            material = new Material(Shader.Find(shaderName));
            material.SetTexture(MainTex,renderTexture);
        }

        void CreateUIDocument()
        {
            uiDocument = gameObject.GetOrAdd<UIDocument>();
            uiDocument.panelSettings = panelSettings;
            uiDocument.visualTreeAsset = visualTreeAsset;
        }

        void CreatePanelSettings()
        {
            panelSettings = Instantiate(panelSettingsAsset);
            panelSettings.targetTexture = renderTexture;
            panelSettings.clearColor = true;
            panelSettings.scaleMode = PanelScaleMode.ConstantPixelSize;
            panelSettings.scale = panelScale;
            panelSettings.name = $"{name} - PanelSettings";
        }

        void CreateRenderTexture()
        {
            RenderTextureDescriptor descriptor = renderTextureAsset.descriptor;
            descriptor.width = panelWidth;
            descriptor.height = panelHeight;
            renderTexture = new RenderTexture(descriptor)
            {
                name = $"{name} - RenderTexture",
            };
        }
        void InitializeComponents()
        {
            InitializeMeshRenderer();
            
            //Optionally add a box collider to the object
            //BoxCollider boxCollider = gameObject.GetOrAdd<BoxCollider>();
            //boxCollider.size = new Vector3(1, 1, 0);
            
            MeshFilter meshFilter = gameObject.GetOrAdd<MeshFilter>();
            meshFilter.sharedMesh = GetQuadMesh();
        }
        
        void InitializeMeshRenderer()
        {
            meshRenderer = gameObject.GetOrAdd<MeshRenderer>();
            meshRenderer.sharedMaterial = null;
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;
            meshRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
            meshRenderer.lightProbeUsage = LightProbeUsage.Off;
            meshRenderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
        }

        static Mesh GetQuadMesh()
        {
            GameObject tempQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            Mesh quadMesh = tempQuad.GetComponent<MeshFilter>().sharedMesh;
            Destroy(tempQuad);
            
            return quadMesh;
        }
    }
}