using Reflex.Core;
using Unity.Behavior.GraphFramework;
using UnityEngine;
namespace Downey.Reflex
{
    public class ProjectInstaller : MonoBehaviour,IInstaller
    {
        public void InstallBindings(ContainerBuilder builder)
        {
            builder.AddSingleton("Hello");
            //builder.AddSingleton(new Logger(),typeof(Logger)); // Register instance as ILogger singleton
            //builder.AddSingleton(typeof(Logger),typeof(ILogger)); // Lazily construct Logger,bind as ILogger
            //builder.AddSingleton(typeof(Logger),typeof(Logger),typeof(ILogger)); // Construct Logger,bind as Logger and ILogger
            //builder.AddSingleton<Logger>(c => new Logger(),typeof(ILogger));//Construct via factory,bind as ILogger singleton
            
            builder.AddTransient(typeof(Logger), typeof(ILogger)); // A new Logger every time
            //builder.AddTransient(new Logger(), typeof(ILogger)); // INVALID : AddTransient does not accept instance
            
            //builder.AddScoped(typeof(Logger), typeof(ILogger)); // One Logger per container
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class Logger : ILogger
    {
        private readonly SerializableGUID id = SerializableGUID.Generate();

        public void Log(string message)
        {
            Debug.Log($"[Singleton Logger: {id}] {message}");
        }
    }
}