namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System.Collections.Generic;

    public interface ICompositeCommand : ICommand, IEnumerable<ICommand>
    {
    }

    public interface ICompositeCommand<out TCommand> : ICommand, IEnumerable<TCommand>
        where TCommand : ICommand
    {
    }
}
