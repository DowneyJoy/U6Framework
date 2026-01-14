using Unity.Properties;
using UnityEngine;

namespace Downey.Property
{
    public static class DebugUtilities
    {
        public static void PrintBindablePaths<T>(T container)
        {
            var visitor = new GatherBindablePropertiesVisitor();
            PropertyContainer.Accept(visitor, ref container);
            
            Debug.Log($"Found {visitor.BindableProperties.Count} Bindable Properties:");
            foreach (var path in visitor.BindableProperties)
            {
                Debug.Log(path.ToString());
            }
        }
    }
}