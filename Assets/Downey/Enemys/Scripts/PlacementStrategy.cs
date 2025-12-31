using UnityEngine;
using UnityUtils;

namespace Refactoring.Patterns
{
    [CreateAssetMenu(fileName = "CirclePlacer", menuName = "Placements/CirclePlacer")]
    public class RandomCirclePlacer : PlacementStrategy
    {
        public float minDistance;
        public float maxDistance = 10f;

        public override Vector3 SetPosition(Vector3 origin)
        {
            return origin.RandomPointInAnnulus(minDistance, maxDistance);
        }
    }
    [CreateAssetMenu(fileName = "DefaultPlacer", menuName = "Placements/DefaultPlacer")]
    public class PlacementStrategy : ScriptableObject
    {
        public virtual Vector3 SetPosition(Vector3 origin) => origin;
    }

}
