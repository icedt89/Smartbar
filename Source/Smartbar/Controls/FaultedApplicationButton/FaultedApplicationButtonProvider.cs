namespace JanHafner.Smartbar.Controls.FaultedApplicationButton
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(IFaultedApplicationButtonProvider))]
    internal sealed class FaultedApplicationButtonProvider : IFaultedApplicationButtonProvider
    {
        [NotNull] 
        private readonly IWindowService windowService;

        [NotNull] 
        private readonly IEventAggregator eventAggregator;

        [NotNull] 
        private readonly ICommandDispatcher commandDispatcher;

        [ImportingConstructor]
        public FaultedApplicationButtonProvider([NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ICommandDispatcher commandDispatcher)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.commandDispatcher = commandDispatcher;
        }

        public Boolean CanCreateApplicationButton(Application application)
        {
            throw new NotSupportedException("This method is not supported in the context where the implementation is intented to be used!");
        }

        public ApplicationButton CreateApplicationButton(Application application)
        {
            var viewModel = new FaultedApplicationViewModel(application, this.windowService,
                this.eventAggregator, this.commandDispatcher);

            return new FaultedApplicationButton(viewModel);
        }
    }
}
