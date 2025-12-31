using UnityEngine;
using UnityEngine.Profiling;
using ZLinq;

namespace Downey.ZLinq
{
    public class Example : MonoBehaviour
    {
        private static readonly int[] source = new[] { 1, 2, 3, 4, 5 };
        public Transform Origin;
        void Start()
        {
            Profiler.logFile = "zlinq.log";
            Profiler.enableBinaryLog = true;
            Profiler.enabled = true;

            var result = source
                .AsValueEnumerable()
                .Where(static x => x % 2 == 0)
                .Select(static x => x * x);
            
            Debug.Log(result);
            Debug.Log(string.Join(", ", result.AsEnumerable()));

            Profiler.enabled = false;

            foreach (var item in result)
            {
                Debug.Log(item);
            }
            
            Debug.Break();
        }

        void GetAncestorsAndDecendants()
        {
            Debug.Log("Ancestors ----------------");
            foreach (var item in Origin.Ancestors()) Debug.Log(item.name);
            
            Debug.Log("Descendants --------------");
            foreach (var item in Origin.Descendants()) Debug.Log(item.name);

            var tagged = Origin.Descendants().Where(x => x.tag == "foobar");

            var scripts = Origin.ChildrenAndSelf().OfComponent<Rigidbody>();
            foreach (var script in scripts) Debug.Log(script.name);
        }
    }
}