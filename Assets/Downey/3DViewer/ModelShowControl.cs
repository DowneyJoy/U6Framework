using UnityEngine;
namespace Downey._3DViewer
{
    public class ModelShowControl : MonoBehaviour
    {
        [SerializeField][Header("渲染展示相机")]
        private GameObject showCamera;
        [SerializeField][Header("展示交互的物品")]
        private GameObject showGameObject;
        
        
        [SerializeField][Header("是否自动旋转")]
        private bool isAutoRotate;
        [SerializeField][Header("自动旋转方向，例如(0,1,0)按照Y轴旋转")]
        private Vector3 autoRotateDirection;
        [SerializeField][Header("是否按照自身坐标系轴自动旋转")]
        private bool isAutoRotatePivot;
        [SerializeField][Header("自动旋转速度")]
        private float autoRotationSpeed = 5; 
        [SerializeField][Header("鼠标旋转速度")]
        private float mouseRotateSpeed = 0.5f;
        [SerializeField][Header("是否反向旋转X")]
        private bool isRotateInvertX;
        [SerializeField][Header("是否反向旋转Y")]
        private bool isRotateInvertY;
        [SerializeField][Header("缩放速度")]
        private float zoomSpeed = 0.5f;
        [SerializeField][Header("是否反向缩放")]
        private bool isZoomInvert;
        [SerializeField][Header("最近缩放距离")]
        private float minZoomDistance = 1f;
        [SerializeField][Header("最远缩放距离")]
        private float maxZoomDistance = 10f;
        
        private Vector3 cameraInitPos;
        private Vector3 lastMousePosition;
        private bool isRotating; //是否正在旋转
        private Vector3[] showGameObjectInitTrans = new Vector3[3];
        
        public static ModelShowControl Instance;
        private void Awake()
        {
            Instance = this;
            cameraInitPos = showCamera.transform.position;
        }
     
     
     
        public void SetShowGameObject(GameObject _gameObject)
        {
            showGameObjectInitTrans[0] = _gameObject.transform.position;
            showGameObjectInitTrans[1] = _gameObject.transform.eulerAngles;
            showGameObjectInitTrans[2] = _gameObject.transform.localScale;
            showGameObject = _gameObject;
            showCamera.transform.position = cameraInitPos;
        }
     
        public void RestoreShowGameObject()
        {
            showGameObject.transform.position = showGameObjectInitTrans[0];
            showGameObject.transform.eulerAngles = showGameObjectInitTrans[1];
            showGameObject.transform.localScale = showGameObjectInitTrans[2];
            showGameObject = null;
            showCamera.transform.position = cameraInitPos;
            isRotating = false;
        }
     
        void Update()
        {
            if (showGameObject == null)
                return;
     
            if (isAutoRotate && !Input.GetMouseButton(0)) // 如果鼠标左键没按下
                RotateModelContinuously(); // 持续旋转模型
            else
                RotateModelOnMouseDrag(); // 根据鼠标拖拽旋转模型
     
            ZoomCamera();
        }
     
        void RotateModelContinuously()
        {
            if (!isAutoRotatePivot)
                showGameObject.transform.Rotate(autoRotateDirection, autoRotationSpeed * Time.deltaTime, Space.World);
            else
            {
                var rotateEuler = autoRotateDirection * autoRotationSpeed * Time.deltaTime;
                // 根据模型当前朝向构造一个围绕轴旋转的四元数
                Quaternion deltaRotation = Quaternion.Euler(rotateEuler);
                // 将新的旋转应用到模型
                showGameObject.transform.rotation *= deltaRotation;
            }
        }
     
        void RotateModelOnMouseDrag()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isRotating = true;
                lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isRotating = false;
            }
     
            if (isRotating && (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0))
            {
                Vector3 currentMousePosition = Input.mousePosition;
                Vector3 mouseDeltaPosition = currentMousePosition - lastMousePosition;
            
                float rotationX = -mouseDeltaPosition.y * mouseRotateSpeed;
                float rotationY = mouseDeltaPosition.x * mouseRotateSpeed;
                showGameObject.transform.Rotate(Vector3.up, isRotateInvertX ? rotationY : -rotationY, Space.World);
                showGameObject.transform.Rotate(Vector3.right, isRotateInvertY ? rotationX : -rotationX, Space.World);
     
                lastMousePosition = currentMousePosition;
            }
        }
     
        void ZoomCamera()
        {
            // 获取鼠标滚轮的滚动值
            float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheelInput == 0) return;
            // 计算缩放后的相机距离
            float zoomDistance = showCamera.transform.localPosition.z -
                                 scrollWheelInput * (isZoomInvert ? zoomSpeed : -zoomSpeed);
            zoomDistance = Mathf.Clamp(zoomDistance, -maxZoomDistance, -minZoomDistance);
     
            // 设置相机距离
            showCamera.transform.localPosition = new Vector3(cameraInitPos.x, cameraInitPos.y, zoomDistance);
        }
    }
}