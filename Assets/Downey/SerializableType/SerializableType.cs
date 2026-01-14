using System;
using UnityEngine;

namespace Downey.SerializableType
{
    [Serializable]
    public class SerializableType : ISerializationCallbackReceiver
    {
        [SerializeField] string assemblyQualifiedName = string.Empty;
        public Type Type { get; private set; }
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            assemblyQualifiedName = Type?.AssemblyQualifiedName??assemblyQualifiedName;
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (!TryGetType(assemblyQualifiedName, out var type))
            {
                Debug.LogError($"Type {assemblyQualifiedName} could not be found.");
                return;
            }
            Type = type;
        }

        static bool TryGetType(string typeString, out Type type)
        {
            type = Type.GetType(typeString);
            return type != null || !string.IsNullOrEmpty(typeString);
        }
    }
}