using UnityEngine;
using UnityEngine.Playables;

namespace Downey.ExtendingTimeline
{
    public class LightControlMixer : PlayableBehaviour
    {
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Light light = playerData as Light;
            if (light == null) return;
            
            Color blendedColor = Color.black;
            float blendedIntensity = 0f;
            float totalWeight = 0f;
                
            int inputCount = playable.GetInputCount();
            for (int i = 0; i < inputCount; i++)
            {
                float weight = playable.GetInputWeight(i);
                var inputPlayable = (ScriptPlayable<LightControlBehaviour>)playable.GetInput(i);
                var input = inputPlayable.GetBehaviour();
                
                blendedColor += input.color * weight;
                blendedIntensity += input.intensity * weight;
                totalWeight += weight;
            }

            if (totalWeight > 0f)
            {
                light.color = blendedColor;
                light.intensity = blendedIntensity;
            }
        }
    }
}