namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface ICommandDispatcher
    {
        Task DispatchAsync([NotNull] IEnumerable<ICommand> commands);
    }
}
