namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface ICommandHandler
    {
        Task HandleAsync([NotNull] ICommand command);

        Task<Boolean> CanHandleAsync([NotNull] ICommand command);
    }
}
