using DG.Tweening;
using Downey.HSM;
using UnityEngine;

namespace Downey.Platformer
{
    public class PlatformMover:MonoBehaviour
    {
        [SerializeField]Vector3 moveTo =  Vector3.zero;
        [SerializeField] private float moveTime = 1f;
        [SerializeField] private Ease ease = Ease.InOutQuad;

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
            Move();
        }

        void Move()
        {
            transform.DOMove(startPosition + moveTo, moveTime)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}