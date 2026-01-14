using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Downey._3DViewer
{
    public class ToolBox : Singleton<ToolBox>
{
    //private ToolMono toolMono;

    //public ToolMono ToolMono
    //{
    //    get {
    //        if (!toolMono) 
    //            return null;
    //        return toolMono;
    //    }
    //    set { toolMono = value; }
    //}

    /// <summary>
    /// 判定鼠标是否悬停在指定的ui上面（不判定ui是否被挡住）
    /// </summary>
    /// <param name="rect">指定ui的rect组件</param>
    /// <returns></returns>
    public bool IsOverUI(RectTransform rect)
    {
        if (!rect) return false;

        if (rect.gameObject.activeInHierarchy && RectTransformUtility.RectangleContainsScreenPoint(rect, Input.mousePosition))
            return true;
        return false;
    }

    private PointerEventData eventData = new PointerEventData(EventSystem.current);
    private List<RaycastResult> raycastResults = new List<RaycastResult>();

    /// <summary>
    /// 判定鼠标是否停在指定ui上面（只会判定能被点击到的ui）
    /// </summary>
    /// <param name="go">指定uiGameObject</param>
    /// <returns></returns>
    public bool IsOverUI(GameObject go) {
        if (!go) return false;
        eventData.position = Input.mousePosition;
        raycastResults.Clear();
        //向点击位置发射一条射线，检测是否点击UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            foreach (RaycastResult result in raycastResults) {
                if (go.Equals(result.gameObject))
                    return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 判定鼠标是否停在指定ui(ui在最上层)上面
    /// </summary>
    /// <param name="go">指定uiGameObject</param>
    /// <returns></returns>
    public bool IsOverUIOnTop(GameObject go)
    {
        if (!go) return false;
        eventData.position = Input.mousePosition;
        raycastResults.Clear();
        //向点击位置发射一条射线，检测是否点击UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            if (go.Equals(raycastResults[0].gameObject))
                return true;
            return false;
        }
        else
        {
            return false;
        }
    }
}

}
