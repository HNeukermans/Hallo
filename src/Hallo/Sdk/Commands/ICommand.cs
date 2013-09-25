namespace Hallo.Sdk.Commands
{
    internal interface ICommand
    {
        void Execute();
    }

    internal interface ICommand<T>
    {
        void Execute(T argument);
    }
}