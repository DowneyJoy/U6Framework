using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Downey.ExtendingTimeline
{
    [TrackClipType(typeof(LightControlAsset))]
    [TrackBindingType(typeof(Light))]
    public class LightControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<LightControlMixer>.Create (graph, inputCount);
        }
    }
}