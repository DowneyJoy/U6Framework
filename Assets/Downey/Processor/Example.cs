using System;
using UnityEngine;

namespace Downey.Processor
{
    public class Example : MonoBehaviour
    {
        private ProcessorDelegate<Vector3, bool> shouldAttack;
        private ProcessorDelegate<Vector3, bool> highlightClosed;
        [Range(0.01f,1f),SerializeField] float distanceThreshold = 0.1f;
        public Transform target;
        private void Start()
        {
            Advanced();
        }
        
        void Update() => highlightClosed.Invoke(target.position);

        public void Simple()
        {
            ProcessorDelegate<Vector3, bool> simpleChain = Chain<Vector3, float>
                .Start(new DistanceFromPlayer(transform))
                .Then(new DistanceScorer())
                .Then(new ThresholdFilter(() => distanceThreshold))
                .Compile();
        }

        public void Advanced()
        {
            shouldAttack = Chain.Start(new DistanceFromPlayer(transform))
                .Then(new DistanceScorer())
                .WithMaxDistance(15f)
                .Then(new ThresholdFilter(() => distanceThreshold))
                .LogTo("Score")
                .LogTo("Proximity")
                .Compile();
            bool result = shouldAttack.Invoke(new Vector3(25, 0, 0));
            Debug.Log($"Should attack at (25,0,0)? {result}");

            highlightClosed = Chain.Start(new DistanceFromPlayer(transform))
                .Then(new DistanceScorer())
                .Then(new ThresholdFilter(() => distanceThreshold))
                .Then(new HighlightIfClose(transform))
                .Compile();
        }
    }
}