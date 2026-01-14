using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Downey._3DViewer
{
    public class MainController : MonoBehaviour
    {
        [Header("ShowModelRawImage脚本")]
        public ShowModelRawImage showModelRawImage;
        [Header("渲染模型的相机")]
        public Camera modelCamera;
        [Header("需要渲染的模型")]
        public GameObject model;

        // Start is called before the first frame update
        void Start()
        {
            //直接调用ShowModelRawImage脚本的Init函数，将模型渲染相机和需要渲染的模型传入参数
            showModelRawImage.Init(modelCamera, model);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

}
