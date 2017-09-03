namespace JanHafner.Smartbar.Views.MainWindow
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Infrastructure.Commanding.Groups;
    using JanHafner.Smartbar.Services;
    using JanHafner.Smartbar.Views.Group.CreateGroup;
    using Prism.Commands;

    internal sealed class MainWindowViewModelCreateGroupCommand : DelegateCommand
    {
        public MainWindowViewModelCreateGroupCommand(ICommandDispatcher commandDispatcher, IWindowService windowService)
            : base(async () =>
            {
                var createGroupViewModel = new CreateGroupViewModel(windowService);
                await windowService.ShowWindowAsync<CreateGroup>(createGroupViewModel, async result =>
                {
                    if (result == MessageBoxResult.OK)
                    {
                        await
                       commandDispatcher.DispatchAsync(new CreateGroupCommand(Guid.NewGuid(),
                           createGroupViewModel.GroupName));
                    }
                });
            })
        {
        }
    }
}
