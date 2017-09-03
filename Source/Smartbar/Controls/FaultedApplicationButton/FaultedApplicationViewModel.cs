namespace JanHafner.Smartbar.Controls.FaultedApplicationButton
{
    using System;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Events;

    internal sealed class FaultedApplicationViewModel : ApplicationViewModel
    {
        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        public FaultedApplicationViewModel([NotNull] Application faultedApplication, [NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ICommandDispatcher commandDispatcher)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (faultedApplication == null)
            {
                throw new ArgumentNullException(nameof(faultedApplication));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            this.Id = faultedApplication.Id;
            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.commandDispatcher = commandDispatcher;
        }

        public Guid Id { get; private set; }

        [NotNull]
        public System.Windows.Input.ICommand MouseEnterApplicationButtonCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.eventAggregator.GetEvent<UpdateStatusbarText>().Publish(Localization.FaultedApplicationButton.FaultedApplicationStatusText);
                });
            }
        }

        [NotNull]
        public System.Windows.Input.ICommand MouseLeaveApplicationButtonCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.eventAggregator.GetEvent<UpdateStatusbarText>().Publish(String.Empty);
                });
            }
        }

        [NotNull]
        public System.Windows.Input.ICommand DeleteFaultedApplicationCommand
        {
            get
            {
                return new FaultedApplicationViewModelDeleteCommand(this, this.windowService, this.commandDispatcher);
            }
        }
    }
}
