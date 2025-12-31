using UnityEngine;
using UnityEngine.Playables;

namespace Downey.ExtendingTimeline
{
    public class LightControlAsset : PlayableAsset
    {
        public Color color = Color.white;
        public float intensity = 1f;

        // public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        // {
        //     var playable = ScriptPlayable<LightControlBehaviour>.Create (graph);
        //     LightControlBehaviour lightControlBehaviour = playable.GetBehaviour();
        //     lightControlBehaviour.intensity = intensity;
        //     lightControlBehaviour.color = color;
        //     return playable;
        // }
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) =>
            ScriptPlayable<LightControlBehaviour>.Create(graph,new LightControlBehaviour{color = color, intensity = intensity});
    }
}