namespace JanHafner.Smartbar.ProcessApplication.ProcessApplicationButton
{
    using System;
    using System.Windows;
    using Common;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Localization;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    internal sealed class ApplicationViewModelDeleteCommand : DelegateCommand
    {
        public ApplicationViewModelDeleteCommand(ProcessApplicationButtonViewModel processApplicationViewModel,
            ISmartbarSettings smartbarSettings, IWindowService windowService,
            ICommandDispatcher commandDispatcher)
            : base(async () =>
            {
                if (!smartbarSettings.DeleteWithConfirmation ||
                    await windowService.ShowSimpleQuestionDialog(
                        String.Format(Dialogs.DeleteApplicationDialogDescriptionText,
                            processApplicationViewModel.Name), Dialogs.DeleteApplicationDialogTitle,
                        MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await commandDispatcher.DispatchAsync(new DeleteApplicationCommand(processApplicationViewModel.Id));
                }
            })
        {
        }
    }
}
