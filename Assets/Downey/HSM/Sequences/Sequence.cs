using System.Threading;
using System.Threading.Tasks;

namespace Downey.HSM
{
    public interface ISequence
    {
        bool IsDone { get; }
        void Start();
        bool Update();
    }
    
    //One activity operation (activate OR deactivate) to run for this phase.
    public delegate Task PhaseStep(CancellationToken ct);

    public class NoopPhase : ISequence
    {
        public bool IsDone { get; private set; }
        public void Start() => IsDone = true; // complete immediately
        public bool Update() => IsDone;
    }
}