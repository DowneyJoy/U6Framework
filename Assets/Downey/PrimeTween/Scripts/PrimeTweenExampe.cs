using UnityEngine;
using PrimeTween;
using UnityUtils;

public class PrimeTweenExample : MonoBehaviour
{
    [Header("Example")]
    [SerializeField] TweenSettings<float> yPositionTween;
    [SerializeField] ShakeSettings shakeSettings;
    [SerializeField] private bool invert;
    [SerializeField] AnimationCurve customCurve;
    [Header("Example1")]
    [SerializeField] private Renderer cubeRenderer;
    [SerializeField] private float duration = 1f;
    
    Tween tween;
    Sequence sequence;
    Material mat;
    void Start()
    {
        
    }

    void Example()
    {
        Tween.PositionY(transform, yPositionTween.WithDirection(invert));

        //Tween.ShakeLocalPosition(transform, strength: new Vector3(0, 1), duration: 1, frequency: 10);
        Tween.ShakeLocalPosition(transform, shakeSettings);

        Tween.PositionX(transform, 2f, 1f, customCurve);

        //Tween.ShakeCamera(Camera.main, strengthFactor: 0.5f);
        Tween.ShakeCamera(Camera.main, strengthFactor: 1.0f, duration: 0.5f, frequency: 10);
    }
    void Example1()
    {
        var ac = new AllocCounter();

        tween = Tween.LocalPositionY(transform, 3f, 2f, Ease.InOutSine);

        sequence = Sequence.Create(cycles: 4, Sequence.SequenceCycleMode.Restart)
            // Grouped animations run in parallel
            .Group(Tween.PositionX(transform, 2f, duration))
            .Group(Tween.Scale(transform, 2f, duration * 0.5f, startDelay: 0.5f))
            // Chained animation starts when group above finish
            .Chain(Tween.Rotation(transform, new Vector3(0, 0, 45), duration))
            // Add a 1-second delay in the chain
            .ChainDelay(1)
            // Callback at the end of each cycle
            .ChainCallback(() => Debug.Log("Cycle completed!"))
            .OnComplete(() => Debug.Log("Sequence fully completed!"))
            // Insert overlapping custom color tween at 0.5s mark
            .Insert(
                0.5f,
                Tween.Custom(
                    target:this,
                    mat.color,
                    Color.red,
                    duration:2f,
                    (target,val) => target.mat.color = val
                ));
    }
    void Update()
    {
        #region Example1
        // if (sequence.isAlive)
        // {
        //     
        // }
        // else return;
        //
        // if(Input.GetKeyDown(KeyCode.S)) sequence.Stop(); // Like dotwwen.Kill(false):stop immediately,keep current value,skips Oncomplete()
        // if(Input.GetKeyDown(KeyCode.C)) sequence.Complete(); // like dotween.Kill(true):jumps to endvalue,trigger Oncomplelte()
        // if(Input.GetKeyDown(KeyCode.P)) sequence.isPaused = !sequence.isPaused;
        #endregion
        
    }
}
