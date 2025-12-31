using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PrimeTween;
using UnityEditor.PackageManager.UI;
using UnityEngine.UI;
namespace Downey.PrimeTween
{
public class TweensSc : MonoBehaviour
{
    private Tween t;
    public Camera camera;
    [SerializeField] RectTransform window; // 绑定窗口组件
    private Image image;
 
    void Update()
    {
        Readme();
    }
 
    public void Readme()
    {
        ///<summary>物体动画</summary>
        #region
        // Y+10
        if (Input.GetKeyDown(KeyCode.A)) Tween.PositionY(transform, endValue: 10, duration: 1, ease: Ease.InOutSine);
        // Rotate 90°
        if (Input.GetKeyDown(KeyCode.S)) Tween.Rotation(transform, endValue: Quaternion.Euler(0, 90, 0), duration: 1);
        // Rotate 360°
        if (Input.GetKeyDown(KeyCode.D)) Tween.EulerAngles(transform, startValue: Vector3.zero, endValue: new Vector3(0, 360), duration: 1);
        #endregion
 
        ///<summary>摄像机动画</summary>
        #region
        // Shake 1
        if (Input.GetKeyDown(KeyCode.Q)) Tween.ShakeCamera(camera, strengthFactor: 0.5f);
        // Shake 2
        if (Input.GetKeyDown(KeyCode.W)) Tween.ShakeCamera(camera, strengthFactor: 1.0f, duration: 0.5f, frequency: 10);
        // Shake Object 
        if (Input.GetKeyDown(KeyCode.E)) Tween.ShakeLocalPosition(transform, strength: new Vector3(0, 1), duration: 1, frequency: 10);
        // Shake Object Left To Right
        if (Input.GetKeyDown(KeyCode.R)) Tween.ShakeLocalRotation(transform, strength: new Vector3(0, 0, 15), duration: 1, frequency: 10);
        // Shake Object Top To Bottom
        var punchDir = transform.up;
        if (Input.GetKeyDown(KeyCode.T)) Tween.PunchLocalPosition(transform, strength: punchDir, duration: 0.5f, frequency: 10);
        #endregion
 
 
        ///<summary>序列动画</summary>
        #region
        // one To two or to three aniamtion
        if (Input.GetKeyDown(KeyCode.Z)) Tween.Position(transform, endValue: new Vector3(10, 0), duration: 1)
            .OnComplete(() => Tween.ShakeCamera(camera, strengthFactor: 0.5f));//OnComplete可以序列动画
 
        // Trun Small To Destory Object
        if (Input.GetKeyDown(KeyCode.X)) Tween.Scale(transform, endValue: 0, duration: 1, endDelay: 0.5f)
            .OnComplete(() => Destroy(gameObject));
 
        //(委托0分配内存)<提高性能的动画方法>
        //Tween.Position(transform, new Vector3(10, 0), duration: 1)
        //.OnComplete(target: this, target => target.SomeMethod());
        #endregion
 
 
        ///<summary>延迟手段</summary>
        #region
        //Delay 1 秒
        if (Input.GetKeyDown(KeyCode.P)) Tween.Delay(duration: 1f, () => Debug.Log("Delay completed"));
 
        #endregion
 
 
        ///<summary>循环动画</summary>
        #region
        //循环动画：把 and 参数传递给方法。将 cycles 设置为-1就会无限期地重复补间。
        if (Input.GetKeyDown(KeyCode.O)) Tween.PositionY(transform, endValue: 10, duration: 0.5f, cycles: 2, cycleMode: CycleMode.Yoyo);
 
 
 
        //循环Sequence按需播放然后循环
        if (Input.GetKeyDown(KeyCode.I))
            Sequence.Create(cycles: 2, CycleMode.Yoyo)
            .Chain(Tween.PositionX(transform, 10, duration: 1))
            .Chain(Tween.PositionY(transform, 20, duration: 1));
 
        #endregion
 
 
        ///<summary>补间</summary>
        #region
        // 补间：object的 LocalPositionX 从当前位置移动到 1.5，持续时间为 1 秒
        Tween tween = Tween.LocalPositionX(transform, endValue: 1.5f, duration: 1f);
 
        // 检查动画是否还在运行（isAlive == true 表示动画正在运行）
        if (tween.isAlive)
        {
            // 如果动画还在运行，打印当前已运行的时间
            Debug.Log($"动画还在运行，已运行时间: {tween.elapsedTime} 秒");
        }
 
        // 停止动画
        tween.Stop();
 
        // 停止所有与目标对象相关的动画
        Tween.StopAll(onTarget: transform);
 
        // 立刻完成动画（跳到终点，并设置为目标值）
        tween.Complete();
 
        // 立刻完成所有与目标对象相关的动画
        Tween.CompleteAll(onTarget: transform);
 
        // 暂停动画
        tween.isPaused = true;
 
        // 批量暂停所有与目标对象相关的动画
        Tween.SetPausedAll(true, onTarget: transform);
 
        // 恢复动画（取消暂停）
        tween.isPaused = false;
 
        // 批量恢复所有与目标对象相关的动画
        Tween.SetPausedAll(false, onTarget: transform);
 
        // 手动设置动画的已运行时间（单位为秒）
        tween.elapsedTime = 0.5f;
 
        // 手动设置动画的进度（归一化值，范围为 0 到 1）
        tween.progress = 0.5f;
 
        // 调整动画的速度（例如 2 倍速或 0.5 倍速）
        tween.timeScale = 2f;
        
        // 在创建新动画之前，停止之前的动画以避免重复创建
        tween.Stop();
 
        // 或者通过目标对象批量停止所有相关的动画
        Tween.StopAll(onTarget: window);
        #endregion
 
        ///<summary>自定义补间</summary>
        #region
        float floatField; // 用于存储浮点数
        Color colorField; // 用于存储颜色
        //   - startValue: 起始值（0）- endValue: 结束值（10）- duration: 动画持续时间（1 秒）- onValueChange: 每次动画更新时调用的回调函数，将新值赋给 floatField
        Tween.Custom(0, 10, duration: 1, onValueChange: newVal => floatField = newVal);
        Tween.Custom(Color.white, Color.black, duration: 1, onValueChange: newVal => colorField = newVal);
        #endregion
 
        ///<summary>时间表</summary>
        #region
        
        //// 创建一个动画并设置其初始 timeScale
        //var tween = Tween.PositionX(transform, endValue: 10f, duration: 2f);
        
        //// 1.平滑地改变动画的 timeScale
        ////   - tween: 目标动画 - newTimeScale: 新的时间缩放值（2f 表示加速两倍）- duration: 平滑过渡的时间（1 秒）
        //// 此方法会逐渐改变动画的速度
        //Tween.TweenTimeScale(tween, newTimeScale: 2f, duration: 1f);
 
        //// 2.平滑地改变全局 Unity 的 Time.timeScale
        ////   - newTimeScale: 新的全局时间缩放值（0.5f 表示减慢一半速度）- duration: 平滑过渡的时间（1 秒）
        //// 此方法会影响所有基于时间的功能（如动画、物理等）
        //Tween.GlobalTimeScale(newTimeScale: 0.5f, duration: 1f);
 
        #endregion
 
        ///<summary>序列</summary>
        #region
        Sequence.Create(cycles: 10, CycleMode.Yoyo)
            .Group(Tween.PositionX(transform, endValue: 10f, duration: 1.5f))
            .Group(Tween.Scale(transform, endValue: 2f, duration: 0.5f, startDelay: 1))
            .Chain(Tween.Rotation(transform, endValue: new Vector3(0f, 0f, 45f), duration: 1f))
            .ChainDelay(1)
            .ChainCallback(() => Debug.Log("Sequence cycle completed"))
            .Insert(atTime: 0.5f, Tween.Color(image, Color.red, duration: 0.5f));
 
        // ====================== 注释说明 ======================
        // Sequence.Create(cycles: 10, CycleMode.Yoyo)
        // 创建一个序列，设置循环 10 次，并使用 Yoyo 模式（正向播放完后反向播放）。
 
        // .Group(Tween.PositionX(...))
        // 使用 Group 方法将多个动画分组，分组的动画会同时启动并并行运行。
        // 在这个例子中，PositionX 和 Scale 动画会被并行执行。
 
        // .Chain(Tween.Rotation(...))
        // 使用 Chain 方法将动画链式连接，链式动画会依次运行。
        // 在这个例子中，Rotation 动画会在前面的 PositionX 和 Scale 动画完成后才开始。
 
        // .ChainDelay(1)
        // 在序列中添加一个延迟，延迟时间为 1 秒。
 
        // .ChainCallback(() => Debug.Log(...))
        // 在序列中添加一个回调函数，在指定的时间点执行自定义逻辑。
        // 在这个例子中，当序列的一个循环完成后，会打印一条日志。
 
        // .Insert(atTime: 0.5f, Tween.Color(...))
        // 使用 Insert 方法在序列的指定时间插入动画，插入的动画会与其他动画重叠。
        // 如果插入的动画超出了当前序列的总持续时间，序列的总持续时间会自动延长。
        // 在这个例子中，颜色变化动画会在序列的第 0.5 秒时插入，并与其他动画重叠运行。
 
        // ====================== 核心概念总结 ======================
        // **Group**：将多个动画分组，分组的动画会同时启动并并行运行。
        // **Chain**：将动画链式连接，链式动画会依次运行，前一个动画完成后才会开始下一个动画。
        // **Insert**：在序列的指定时间插入动画，插入的动画会与其他动画重叠运行。
        // **ChainDelay**：在序列中添加延迟，延迟一段时间后再继续执行后续动画。
        // **ChainCallback**：在序列中添加回调函数，在指定的时间点执行自定义逻辑。
        #endregion
 
        ///<summary>未吃透的自定义更新</summary>
        #region
 
        //    // 使用 OnUpdate() 方法在动画值更新时执行自定义逻辑
        //    // ====================== 示例 1：围绕 Y 轴旋转物体 ======================
        //    // 随着动画的进行，让物体围绕 Y 轴旋转
        //    Tween.PositionY(transform, endValue: 5f, duration: 2f)
        //        .OnUpdate(
        //            target: transform, // 指定目标对象（这里是 transform）
        //            (target, tween) => {
        //                // 在每次动画更新时，修改目标对象的 rotation 属性
        //                // 使用 tween.interpolationFactor 获取插值因子（范围为 0 到 1）
        //                target.rotation = Quaternion.Euler(0, tween.interpolationFactor * 90f, 0);
        //            }
        //        );
        //    // 注释说明：
        //    //   - interpolationFactor 是一个归一化的值，表示动画当前进度的比例（0 表示开始，1 表示结束）。
        //    //   - 这里通过 interpolationFactor 计算旋转角度，实现物体随着动画进展逐渐旋转的效果。
 
        //    // ====================== 示例 2：调用自定义方法 ======================
        //    // 在每次位置变化时调用 OnPositionUpdated() 方法
        //    Tween.PositionY(transform, endValue: 5f, duration: 2f)
        //        .OnUpdate(
        //            target: this, // 指定目标对象（这里是当前脚本实例）
        //            (target, tween) => {
        //                // 在每次动画更新时，调用目标对象的 OnPositionUpdated() 方法
        //                // 并将动画的进度传递给该方法
        //                target.OnPositionUpdated(tween.progress);
        //            }
        //        );
        //    // 注释说明：
        //    //   - progress 是一个归一化的值，表示动画当前的完成度（范围为 0 到 1）。
        //    //   - 这里通过 progress 参数调用自定义方法 OnPositionUpdated()，可以在方法中实现额外逻辑。
 
        //            // ====================== 自定义方法示例 ======================
        //            // 定义一个方法，用于处理动画进度
        //    private void OnPositionUpdated(float progress)
        //        {
        //            // 根据动画进度执行某些操作
        //            Debug.Log($"Animation progress: {progress:P1}"); // 打印动画进度（如 50.0%）
        //        }
 
        //// ====================== OnUpdate 的作用 ======================
        //// OnUpdate() 是一个回调机制，允许你在动画更新时执行自定义逻辑。
        //// 它非常适合需要实时响应动画变化的场景，比如动态调整物体状态或触发其他事件。
 
        //// ====================== 关键概念 ======================
        //// - target：指定目标对象，通常是你要操作的对象（如 transform 或脚本实例）。
        //// - lambda 表达式：定义回调逻辑，接收两个参数：
        ////     - target：你指定的目标对象。
        ////     - tween：当前动画的引用，可以通过它访问动画的状态（如 progress、interpolationFactor 等）。
 
        //// ====================== 常用属性 ======================
        //// - interpolationFactor：
        ////     归一化的插值因子，表示动画当前的插值进度（范围为 0 到 1）。
        ////     可以用来计算中间值，例如旋转角度、透明度等。
        //// - progress：
        ////     动画的完成度（范围为 0 到 1），适合用来触发基于进度的逻辑。
 
        //// ====================== 实际用途 ======================
        //// - 实时调整物体状态：比如根据动画进度旋转物体、改变颜色等。
        //// - 触发外部逻辑：比如在动画过程中调用其他方法，或者与其他系统交互。
        //// - 动态反馈：比如在动画更新时打印日志、播放音效、显示进度条等。
        #endregion
 
        ///<summary>未吃透的自定义缓动和动画曲线</summary>
        #region
 
        //// 使用 AnimationCurve 自定义补间动画的缓动效果
 
        //// 创建一个 AnimationCurve，表示缓动曲线
        //AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1); // 默认的缓入缓出曲线
 
        //// 将 AnimationCurve 传递给补间方法
        //Tween.PositionY(transform, endValue: 5f, duration: 2f, ease: animationCurve);
 
        //// 注释说明：
        ////   - AnimationCurve 是 Unity 提供的一种工具，允许你通过编辑器或代码定义复杂的缓动曲线。
        ////   - 在这里，我们使用 AnimationCurve 替代了默认的 Ease 枚举，提供了更灵活的缓动效果。
 
        //// 使用参数化缓动来自定义标准缓动类型的行为
 
        //// ====================== Overshoot 缓动 ======================
        //// 自定义 Ease.OutBack 的过冲强度
        //// 参数 strength 控制过冲的程度（值越大，过冲越明显）
        //Tween.PositionY(transform, endValue: 5f, duration: 2f, ease: Easing.Overshoot(strength: 1.5f));
 
        //// 注释说明：
        ////   - Easing.Overshoot 用于增强“回弹”效果。
        ////   - 适合模拟物体超出目标位置后再返回的效果。
 
        //// ====================== Bounce 缓动 ======================
        //// 自定义 Ease.OutBounce 的弹跳强度
        //// 参数 strength 控制弹跳的幅度（值越大，弹跳越明显）
        //Tween.PositionY(transform, endValue: 5f, duration: 2f, ease: Easing.Bounce(strength: 2f));
 
        //// 注释说明：
        ////   - Easing.Bounce 用于创建弹跳效果。
        ////   - 弹跳的次数和幅度由 strength 参数控制。
 
        //// ====================== BounceExact 缓动 ======================
        //// 自定义第一次反弹的确切幅度（以米/角度为单位）
        //// 参数 amplitude 指定第一次反弹的幅度
        //Tween.PositionY(transform, endValue: 5f, duration: 2f, ease: Easing.BounceExact(amplitude: 1f));
 
        //// 注释说明：
        ////   - Easing.BounceExact 确保第一次反弹的幅度是精确的。
        ////   - 适合需要严格控制反弹高度的场景。
 
        //// ====================== Elastic 缓动 ======================
        //// 自定义 Ease.OutElastic 的强度和振荡周期
        //// 参数 strength 控制弹性强度，period 控制振荡周期
        //Tween.PositionY(transform, endValue: 5f, duration: 2f, ease: Easing.Elastic(strength: 1f, period: 0.3f));
 
        //// 注释说明：
        ////   - Easing.Elastic 用于创建弹性摆动效果。
        ////   - strength 控制摆动的幅度，period 控制摆动的频率。
        //// ====================== 动画曲线（AnimationCurve） ======================
        //// - AnimationCurve 是一种图形化的缓动工具，允许你通过调整曲线点来定义动画的速率变化。
        //// - 它比固定的 Ease 枚举更灵活，可以实现完全自定义的缓动效果。
 
        //// ====================== 参数化缓动 ======================
        //// - 参数化缓动允许你对标准缓动类型进行微调，满足特定需求。
        //// - 常见的参数化缓动包括以下几种：
 
        ////   - Easing.Overshoot：
        ////       增强“回弹”效果，适合模拟物体超出目标位置后再返回的场景。
        ////       参数 strength 控制过冲的程度。
 
        ////   - Easing.Bounce：
        ////       创建弹跳效果，适合模拟物体落地后反弹的场景。
        ////       参数 strength 控制弹跳的幅度。
 
        ////   - Easing.BounceExact：
        ////       精确控制第一次反弹的幅度，适合需要严格控制高度的场景。
        ////       参数 amplitude 指定反弹的高度（单位为米或角度）。
 
        ////   - Easing.Elastic：
        ////       创建弹性摆动效果，适合模拟弹簧或橡皮筋的运动。
        ////       参数 strength 控制摆动的幅度，period 控制摆动的频率。
 
        //// ====================== 实际用途 ======================
        //// - 动画曲线：适合需要复杂、非线性缓动的场景。
        //// - 参数化缓动：适合需要微调标准缓动行为的场景。
        //// - 结合使用：你可以根据需求选择合适的缓动方式，让动画更加生动和自然。
        #endregion
 
        ///<summary>未吃透的延迟更新/固定更新</summary>
        #region
 
        //// 使用 `updateType` 参数选择 Unity 的哪个更新函数来驱动动画
        //// 可用选项包括 Update、LateUpdate 和 FixedUpdate
 
        //// ====================== 示例 1：使用 TweenSettings 设置 LateUpdate ======================
        //// 使用 TweenSettings 或 TweenSettings<T> 结构体将 'updateType' 参数传递给静态方法
        //Tween.PositionX(
        //    transform,
        //    endValue: 10f,
        //    new TweenSettings(duration: 1f, updateType: UpdateType.LateUpdate) // 指定动画在 LateUpdate 中更新
        //);
 
        //// 注释说明：
        ////   - UpdateType.LateUpdate 表示动画会在 Unity 的 LateUpdate 阶段更新。
        ////   - LateUpdate 常用于确保动画在所有其他逻辑（如物理计算或摄像机更新）完成后执行。
 
        //// ====================== 示例 2：使用 TweenSettings<T> 设置 FixedUpdate ======================
        //// 创建一个 TweenSettings<float> 实例，并指定 updateType 为 FixedUpdate
        //var tweenSettingsFloat = new TweenSettings<float>(
        //    endValue: 10f,
        //    duration: 1f,
        //    updateType: UpdateType.FixedUpdate // 指定动画在 FixedUpdate 中更新
        //);
 
        //// 将设置应用到补间动画
        //Tween.PositionX(transform, tweenSettingsFloat);
 
        //// 注释说明：
        ////   - UpdateType.FixedUpdate 表示动画会在 Unity 的 FixedUpdate 阶段更新。
        ////   - FixedUpdate 是固定时间间隔调用的，适合与物理相关的动画（如物体移动或旋转）。
 
        //// ====================== 示例 3：在 Sequence 中使用 FixedUpdate ======================
        //// 如果需要在 Sequence 中使用特定的更新类型，可以通过 Sequence.Create() 方法传递 updateType 参数
        //Sequence.Create(updateType: UpdateType.FixedUpdate); // 指定序列在 FixedUpdate 中更新
 
        //// 注释说明：
        ////   - Sequence.Create() 允许你创建一个序列，并指定其更新类型。
        ////   - 这里的 FixedUpdate 确保整个序列以固定时间间隔更新，适合包含物理相关动画的序列。
 
        //// ====================== 更新类型的作用 ======================
        //// 在 Unity 中，动画可以基于不同的更新函数来驱动：
        ////   - Update：每帧调用一次，适合常规动画。
        ////   - LateUpdate：每帧调用一次，但在所有 Update 和物理计算之后执行，适合与摄像机或 UI 相关的动画。
        ////   - FixedUpdate：以固定时间间隔调用，适合与物理相关的动画（如刚体运动）。
 
        //// ====================== 使用场景 ======================
        //// - Update：
        ////     默认选项，适合大多数动画需求。
        ////     如果没有特殊要求，通常不需要更改更新类型。
 
        //// - LateUpdate：
        ////     适合需要在所有其他逻辑完成后执行的动画。
        ////     例如，摄像机跟随目标物体时，确保目标物体已经完成位置更新。
 
        //// - FixedUpdate：
        ////     适合需要与物理系统同步的动画。
        ////     例如，控制刚体的移动或旋转时，确保动画与物理步长一致。
 
        //// ====================== 如何选择更新类型 ======================
        //// - 如果动画涉及物理计算（如刚体运动），选择 FixedUpdate。
        //// - 如果动画需要在所有其他逻辑完成后执行（如摄像机跟随），选择 LateUpdate。
        //// - 如果动画是普通的视觉效果，使用默认的 Update 即可。
 
        //// ====================== 注意事项 ======================
        //// - 不同的更新类型会影响动画的流畅性和同步性。
        //// - 在复杂项目中，合理选择更新类型可以避免动画与其他系统（如物理或摄像机）之间的冲突。
        #endregion
 
        ///<summary>调试补间防止GC</summary>
        #region
 
        //// 要调试补间，可以在场景层次结构中找到 PrimeTweenManager 对象
        //// PrimeTweenManager 位于 DontDestroyOnLoad 折叠下，允许你检查所有当前正在运行的补间及其属性
 
        //// ====================== 示例：如何快速定位补间的目标对象 ======================
        //// 如果补间有 target 属性（UnityEngine.Object 类型），可以通过单击字段在 Unity 层次结构中快速显示它
        //// 这是为什么即使 target 是可选的，也建议提供它的原因
 
        //// 即使对于不直接操作 GameObject 的补间（如 Tween.Delay() 或 Tween.Custom()），也可以为其指定一个虚拟目标
        //// 这样可以帮助你在调试时更容易识别补间
        //Tween.Delay(1f, target: someGameObject); // 提供一个目标对象以方便调试
 
        //// 在 Inspector 中查看 PrimeTweenManager：
        ////   - 所有正在运行的补间都会列出其详细信息（如目标、持续时间、进度等）
        ////   - 如果补间指定了 target，你可以单击它快速跳转到对应的 GameObject
 
        //// ====================== 最大活动补间数 ======================
        //// 在 Inspector 中，PrimeTween 会显示当前会话中的“最大活动补间数”
        //// 这个数字可以帮助你估计游戏中可能需要的最大补间数量
 
        //// 使用 PrimeTweenConfig.SetTweensCapacity(int capacity) 方法设置最大补间容量
        //// 这会在游戏启动时预先分配足够的内存，确保运行时不会产生额外的内存分配
        //void Start()
        //{
        //    // 设置最大补间容量为 500
        //    PrimeTweenConfig.SetTweensCapacity(500);
        //}
 
        //// 注释说明：
        ////   - 预先分配补间容量可以避免运行时的动态内存分配，从而提高性能。
        ////   - 特别是在性能密集型项目中，这种方法非常有用。
 
        // ====================== 调试补间的作用 ======================
        // 调试补间是优化和排查动画问题的重要工具。
        // 通过 PrimeTweenManager，你可以实时查看所有正在运行的补间及其状态。
 
        // ====================== 快速定位补间目标 ======================
        // - 如果补间有 target 属性（例如某个 GameObject 或 Component），Unity 会在 Inspector 中提供一个快捷链接。
        // - 点击该链接可以直接跳转到对应的 GameObject，方便你快速定位和调试。
        // - 即使某些补间（如 Tween.Delay 或 Tween.Custom）不需要目标对象，也可以为其指定一个虚拟目标。
        //   这样可以让你更容易识别和管理这些补间。
 
        // ====================== 最大活动补间数的意义 ======================
        // - “最大活动补间数”是一个关键指标，表示当前会话中同时运行的补间数量的最大值。
        // - 通过观察这个数字，你可以估算出游戏中可能需要的最大补间容量。
 
        // ====================== 预先分配补间容量的好处 ======================
        // - PrimeTweenConfig.SetTweensCapacity(int capacity) 方法允许你在游戏启动时预先分配补间所需的内存。
        // - 这样做的好处是避免运行时的动态内存分配，从而减少垃圾回收（GC）的压力。
        // - 特别是在频繁创建和销毁补间的场景中（如复杂动画或特效），这种方法可以显著提升性能。
 
        // ====================== 如何使用 ======================
        // 1. 在开发过程中，观察 PrimeTweenManager 中的补间列表，了解每个补间的状态。
        // 2. 估算出游戏中可能需要的最大补间数量，并在游戏启动时调用 SetTweensCapacity 方法。
        // 3. 确保为每个补间提供一个有意义的 target（即使它是可选的），以便于调试和管理。
        #endregion
 
    }
    // 补间示例：如果窗口需要显示，则滑动到 y = 0；如果需要隐藏，则滑动到 y = -500
    public void SetWindowOpened(bool isOpened)
        {
            Tween.UIAnchoredPositionY(window, endValue: isOpened ? 0 : -500, duration: 0.5f);
        }
    }
 
}