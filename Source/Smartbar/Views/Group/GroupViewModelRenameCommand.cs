namespace JanHafner.Smartbar.Views.Group
{
    using System.Windows;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Infrastructure.Commanding.Groups;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.Group.RenameGroup;
    using Prism.Commands;

    internal sealed class GroupViewModelRenameCommand : DelegateCommand
    {
        public GroupViewModelRenameCommand(GroupViewModel groupViewModel, IWindowService windowService, ICommandDispatcher commandDispatcher)
            : base(async () =>
            {
                var renameGroupViewModel = new RenameGroupViewModel(groupViewModel.Name, windowService);
                if (await windowService.ShowWindowAsync<RenameGroup.RenameGroup>(renameGroupViewModel) == MessageBoxResult.OK)
                {
                    await commandDispatcher.DispatchAsync(new RenameGroupCommand(groupViewModel.Id, renameGroupViewModel.NewGroupName));
                }
            })
        {
        }
    }
}
