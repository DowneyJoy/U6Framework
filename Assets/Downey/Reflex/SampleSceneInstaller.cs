using Reflex.Core;
using UnityEngine;

namespace Downey.Reflex
{
    public class SampleSceneInstaller:MonoBehaviour, IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton("World");
        }

    }
}