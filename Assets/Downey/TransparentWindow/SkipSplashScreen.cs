using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
namespace Downey.TransparentWindow
{
    public class SkipSplashScreen : MonoBehaviour
    {
        public SplashScreen.StopBehavior stopBehavior = SplashScreen.StopBehavior.StopImmediate;
    
        IEnumerator Start()
        {
            // 开始渲染启动画面
            SplashScreen.Begin();
        
            // 循环检测按键并跳过
            while (!SplashScreen.isFinished) // 当启动画面未完成渲染时
            {
                if (Input.anyKeyDown) // 检测任意按键
                {
                    SplashScreen.Stop(stopBehavior);
                    break; // 退出循环
                }
                yield return null; // 每帧检测
            }
        }
    }
}