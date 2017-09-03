namespace JanHafner.Smartbar.Controls.EmptyApplicationButton
{
    using System;
    using System.Collections.Generic;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Events;

    internal sealed class EmptyApplicationViewModel : ApplicationViewModel
    {
        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly IUIExtensionService uiExtensionService;

        public EmptyApplicationViewModel([NotNull] IEventAggregator eventAggregator,
            [NotNull] IUIExtensionService uiExtensionService)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (uiExtensionService == null)
            {
                throw new ArgumentNullException(nameof(uiExtensionService));
            }

            this.eventAggregator = eventAggregator;
            this.uiExtensionService = uiExtensionService;
        }

        [NotNull]
        public IEnumerable<IDynamicUICommand> SelectApplicationCreationCommands
        {
            get
            {
                return this.uiExtensionService.CreateCreateApplicationCommands(this.GroupId, this.Column, this.Row, () => true);
            }
        }

        [NotNull]
        public System.Windows.Input.ICommand MouseEnterApplicationButtonCommand
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
    }
}
