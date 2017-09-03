namespace JanHafner.Smartbar.Views.Group
{
    using System;
    using System.Windows;
    using Common;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Infrastructure.Commanding.Groups;
    using JanHafner.Smartbar.Localization;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    internal sealed class GroupViewModelDeleteWithMiddleMouseButtonCommand : DelegateCommand
    {
        public GroupViewModelDeleteWithMiddleMouseButtonCommand(GroupViewModel groupViewModel, ISmartbarSettings smartbarSettings, IWindowService windowService, ICommandDispatcher commandDispatcher)
            : base(async () =>
            {
                if (!smartbarSettings.DeleteWithConfirmation || await windowService.ShowSimpleQuestionDialog(String.Format(Dialogs.DeleteGroupDialogDescriptionText, groupViewModel.Name), Dialogs.DeleteGroupDialogTitle, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await commandDispatcher.DispatchAsync(new DeleteGroupCommand(groupViewModel.Id));
                }
            }, () => smartbarSettings.DeleteGroupWithMiddleMouseButton && groupViewModel.CanBeDeleted)
        {
        }
    }
}
