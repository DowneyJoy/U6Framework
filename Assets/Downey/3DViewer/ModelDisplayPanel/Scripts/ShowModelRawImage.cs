using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Downey._3DViewer
{
    /// <summary>
/// 在图片上显示模型并旋转（挂载到指定鼠标移动的image)
/// （直接调用该脚本的Init函数）
/// </summary>
public class ShowModelRawImage : MonoBehaviour
{
    private Camera modelCamera;//渲染模型的相机
    private GameObject nowModel;

    private Transform originalPos;
    private RectTransform modelPicRect; // 模型图片rect（用于判定鼠标是否在图片上）
    private bool isResetting = false; // 模型回退到原来的状态
    private float originalMouseX = 0.0f;
    private float originalMouseY = 0.0f;
    private float mouseXCount = 0.0f;
    private float mouseYCount = 0.0f;
    private float lerpX = 0.0f;
    private float lerpY = 0.0f;
    private bool isXFinished = false;
    private bool isYFinished = false;
    private bool isScaleFinished = false; // 用于判定是否缩放完成

    [Header("旋转角度恢复原状速度")]
    public float resetSpeed = 5.0f; // 模型恢复原装速度
    [Header("摄像机到模型的距离")]
    public float defaultDistanc = 5.0f;
    [Header("缩放速度")]
    public float zoomSpeed = 1.0f;
    [Header("缩放比例恢复速度")]
    public float resetZoomSpeed = 10.0f;
    [Header("模型放大的最大倍数（摄像机最近的距离）")]
    public float minDistanceRatio = 1.0f; // 摄像机到模型的最小距离等级
    [Header("模型缩小的最大倍数（摄像机最远距离）")]
    public float maxDistanceRatio = 10.0f; // 摄像机到模型的最大距离等级
    [Header("摄像机初始设置距离，代表的倍数（若摄像机距离为10，则缩放到3倍就是距离到10的位置）")]
    public float initDistanceRatio = 3.0f; // 计算规则相当复杂


    private float originalDistance = 0.0f; // 最开始摄像机到模型中心的距离
    private float perDistence = 0.0f; // 每一级缩放的距离

    // originalDistance保存一开始摄像机到模型中心的位置
    // 根据initDistanceRatio为初始设定的距离的放大比例，当前为3倍，也就是初始设定为3倍大小
    // perDistence则保存每一倍大小的距离，使用初始距离除以初始放大倍数可以得到这个值
    // minDistanceRatio和maxDistanceRatio则为最小的放大倍数和最大的放大倍数

    private void Awake()
    {
        modelPicRect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 展示用的摄像机和需要展示的模型（直接调用该函数）
    /// </summary>
    /// <param name="modelCamera">模型渲染相机</param>
    /// <param name="showMedel">模型</param>
    public void Init(Camera newCamera, GameObject showModel) {
        modelCamera = newCamera;
        nowModel = showModel;
        originalPos = modelCamera.transform;
        originalDistance = Vector3.Distance(modelCamera.transform.position, showModel.transform.position);
        perDistence = originalDistance / initDistanceRatio;
    }

    // Update is called once per frame
    void Update()
    {
        if (isResetting)
        {
            if (!isXFinished)
            {
                float rotateSpeedX = lerpX * Time.deltaTime * resetSpeed;
                modelCamera.transform.RotateAround(nowModel.transform.position, Vector3.up, rotateSpeedX);
                mouseXCount += -rotateSpeedX;

                if (Mathf.Abs(mouseXCount - originalMouseX) <= Mathf.Abs(rotateSpeedX))
                {
                    isXFinished = true;
                }
            }

            if (!isYFinished)
            {
                float rotateSpeedY = lerpY * Time.deltaTime * resetSpeed;
                modelCamera.transform.RotateAround(nowModel.transform.position, modelCamera.transform.right, rotateSpeedY);
                mouseYCount += -rotateSpeedY;

                if (Mathf.Abs(mouseYCount - originalMouseY) <= Mathf.Abs(rotateSpeedY))
                {
                    isYFinished = true;
                }
            }

            if (isYFinished && isXFinished)
            {
                originalMouseX = 0.0f;
                originalMouseY = 0.0f;
                mouseXCount = 0.0f;
                mouseYCount = 0.0f;
            }

            if (!isScaleFinished)
            {
                float nowDis = Vector3.Distance(modelCamera.transform.position, nowModel.transform.position);
                float rScaleDir = (nowDis - defaultDistanc >= 0) ? 1 : -1;
                if (nowDis - defaultDistanc <= 0.001f)
                {
                    isScaleFinished = true;
                }
                modelCamera.transform.Translate(Vector3.forward * Time.deltaTime * resetZoomSpeed * rScaleDir);
            }

            if (isYFinished && isXFinished && isScaleFinished)
            {
                modelCamera.transform.position = originalPos.position;
                modelCamera.transform.rotation = originalPos.rotation;
                isXFinished = false;
                isYFinished = false;
                isScaleFinished = false;
                isResetting = false;
            }
            return;
        }

        if (ToolBox.Instance.IsOverUI(modelPicRect))
        {
            if (nowModel == null) return;
            if (Input.GetMouseButton(1))
            {
                float mouse_x = Input.GetAxis("Mouse X");  //获取鼠标X轴移动
                float mouse_y = -Input.GetAxis("Mouse Y");  //获取鼠标Y轴移动

                originalMouseX += mouse_x * 5;
                originalMouseY += mouse_y * 5;

                modelCamera.transform.RotateAround(nowModel.transform.position, Vector3.up, mouse_x * 5);
                modelCamera.transform.RotateAround(nowModel.transform.position, modelCamera.transform.right, mouse_y * 5);
            }

            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                float b = Vector3.Distance(modelCamera.transform.position, nowModel.transform.position);
                float c = Input.GetAxis("Mouse ScrollWheel");  //滑轮滑动
                if (c > 0 && b > originalDistance - (initDistanceRatio - minDistanceRatio) * perDistence)
                    modelCamera.transform.Translate(Vector3.forward * zoomSpeed);

                if (c < 0 && b < originalDistance +  (maxDistanceRatio - initDistanceRatio) * perDistence)
                    modelCamera.transform.Translate(Vector3.forward * -zoomSpeed);
            }
        }
    }
}

}
