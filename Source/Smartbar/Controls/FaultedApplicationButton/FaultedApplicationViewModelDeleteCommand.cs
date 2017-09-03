namespace JanHafner.Smartbar.Controls.FaultedApplicationButton
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Services;
    using Prism.Commands;

    internal sealed class FaultedApplicationViewModelDeleteCommand : DelegateCommand
    {
        public FaultedApplicationViewModelDeleteCommand(FaultedApplicationViewModel faultedApplicationViewModel, IWindowService windowService,
            ICommandDispatcher commandDispatcher)
            : base(async () =>
            {
                if (await windowService.ShowSimpleQuestionDialog(
                    String.Format(Localization.FaultedApplicationButton.DeleteFaultedApplicationDialogDescriptionText,
                        faultedApplicationViewModel.Row, faultedApplicationViewModel.Column),
                    Localization.FaultedApplicationButton.DeleteFaultedApplicationDialogTitle,
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    await commandDispatcher.DispatchAsync(new DeleteApplicationCommand(faultedApplicationViewModel.Id));
                }
            })
        {
        }
    }
}
