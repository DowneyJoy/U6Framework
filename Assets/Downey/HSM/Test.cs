using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Downey.HSM
{
    public class Test : MonoBehaviour
    {
        public ISequence Sequence;
        public CancellationToken ct;

        void Start()
        {
            Sequence = new ParallelPhase(new List<PhaseStep>(), ct);
        }
    
    }

}
