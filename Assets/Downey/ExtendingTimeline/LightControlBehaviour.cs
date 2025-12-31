using UnityEngine;
using UnityEngine.Playables;

namespace Downey.ExtendingTimeline
{
    public class LightControlBehaviour : PlayableBehaviour
    {
        public Color color = Color.white;
        public float intensity = 1f;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Light light = playerData as Light;
            if(light == null)return;
            light.color = color;
            light.intensity = intensity;
        }
    }
}