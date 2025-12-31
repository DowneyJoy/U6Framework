using System.Collections.Generic;
using UnityEngine;

namespace Refactoring.Patterns
{
    public class Enmey : MonoBehaviour, IEntity
    {
        public EnemyConfig Config { get; private set; }
        readonly Queue<ICommand<IEntity>> commandQueue = new Queue<ICommand<IEntity>>();
        
        public void Initialize(EnemyConfig config) =>  Config = config;
        
        public void QueueCommand(ICommand<IEntity> command) => commandQueue.Enqueue(command);
        
        public void ExecuteCommand()
        {
            if (commandQueue.Count > 0)
            {
                commandQueue.Dequeue()?.Execute();
            }
        }

        void Example()
        {
            ExecuteCommand();

            var newCommand = new BattleCommand.Builder(new List<IEntity> { this })
                .WithAction(_ => Debug.Log($"{Config.type} does something."))
                .WithLogger()
                .Build();
            
            QueueCommand(newCommand);
        }
    }
}