namespace JanHafner.Smartbar.ProcessApplication.ProcessApplicationButton
{
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using Prism.Commands;

    internal sealed class ProcessApplicationButtonViewModelExecuteCommand : DelegateCommand
    {
        public ProcessApplicationButtonViewModelExecuteCommand(ProcessApplicationButtonViewModel processApplicationViewModel, ICommandDispatcher commandDispatcher)
            : base(() =>
            {
#pragma warning disable 4014
                commandDispatcher.DispatchAsync(new ExecuteProcessApplicationCommand(processApplicationViewModel.Id));
#pragma warning restore 4014
            })
        {
        }
    }
}
