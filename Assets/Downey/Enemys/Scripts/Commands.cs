using System;
using System.Collections.Generic;
using UnityEngine;

namespace Refactoring.Patterns
{
    public interface ICommand<T>
    {
        void Execute();
    }

    public class CommandLogger : ICommand<IEntity>
    {
        private readonly ICommand<IEntity> command;
        public CommandLogger(ICommand<IEntity> command)
        {
            this.command = command;
        }

        public void Execute()
        {
            Debug.Log($"Command executed: {command.GetType().Name}");
            command.Execute();  
        }
    }
    public class BattleCommand : ICommand<IEntity>
    {
        List<IEntity> targets;
        Action<IEntity> action = delegate { };
        private BattleCommand(){}
        
        public void Execute()
        {
            foreach (var target in targets)
            {
                action.Invoke(target);
            }
        }
        
        public class Builder
        {
            readonly BattleCommand command = new BattleCommand();
            private bool isLoggerEnabled;

            public Builder(List<IEntity> targets = default)
            {
                command.targets = targets ?? new  List<IEntity>();
            }

            public Builder WithAction(Action<IEntity> action)
            {
                command.action = action;
                return this;
            }

            public Builder WithLogger()
            {
                isLoggerEnabled = true;
                return this;
            }
            
            public ICommand<IEntity> Build() => isLoggerEnabled ? new CommandLogger(command) : command;
        }
    }
}