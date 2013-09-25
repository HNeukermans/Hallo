using System;
using Hallo.Sdk.Commands;

namespace Hallo.Sdk.Commands
{
    public class Command<T> : ICommand<T>
    {
        public Action<T> Action { get; private set; }

        public Command(Action<T> action)
        {
            Action = action;
        }

        public void Execute(T argument)
        {
            Action(argument);
        }
    }

    public class Command : ICommand
    {
        public Action Action { get; private set; }

        public Command(Action action)
        {
            Action = action;
        }

        public void Execute()
        {
            Action();
        }
    }
}