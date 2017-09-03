namespace JanHafner.Smartbar.Views.Group
{
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Infrastructure.Commanding.Groups;
    using Prism.Commands;

    internal sealed class GroupViewModelShiftRightCommand : DelegateCommand
    {
        public GroupViewModelShiftRightCommand(GroupViewModel groupViewModel, ICommandDispatcher commandDispatcher)
            : base(async () =>
            {
                await commandDispatcher.DispatchAsync(new RepositionGroupCommand(groupViewModel.Id, groupViewModel.Position + 1));
            }, () => groupViewModel.CanShiftRight)
        {
        }
    }
}
