using Cysharp.Threading.Tasks;
using PrimeTween;
using UnityEngine;
using UnityEngine.Pool;

public class Entity : MonoBehaviour
{
    IObjectPool<Entity> pool;
    Tween tween;
    Sequence anim;
    
    public void Initialize(IObjectPool<Entity> poolRef) => pool = poolRef;

    public async UniTask PlaySpawnAnimationAsync()
    {
        transform.localScale = Vector3.zero;
        anim = Tween.Scale(transform, Vector3.one, 0.5f, Ease.InOutSine)
            .Group(Tween.LocalPositionY(transform, transform.localPosition.y + 1f, 0.5f, Ease.InOutSine));
        await anim;
    }

    public async UniTask MoveAwayAsync(float distance = 3f, float duration = 1.5f)
    {
        var startPos = transform.position;
        var randomDir = new Vector3(Random.Range(-1f,1f),0f,Random.Range(-1f,1f)).normalized;
        var targetPos = startPos + randomDir * distance;
        tween = Tween.Position(transform, targetPos, duration, Ease.InOutSine)
            .OnUpdate(transform, (t, tw) =>
            {
                t.rotation = Quaternion.Euler(0f, Mathf.Sin(tw.elapsedTime * 4f) * 15f, 0f);
            });
        await tween;
    }

    public async UniTask PlayDeathAnimationAsync()
    {
        anim = Tween.Scale(transform,Vector3.zero,0.3f,Ease.InOutSine)
            .Group(Tween.LocalPositionY(transform, transform.localPosition.y - 1f, 0.3f, Ease.InOutSine))
            .OnComplete(ReturnPool);
        await anim;
    }

    void ReturnPool()
    {
        Interrupt();
        pool.Release(this);
    }

    public void Interrupt()
    {
        if(anim.isAlive) anim.Stop();
    }
}
