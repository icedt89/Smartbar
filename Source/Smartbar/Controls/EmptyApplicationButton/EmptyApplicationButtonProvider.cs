namespace JanHafner.Smartbar.Controls.EmptyApplicationButton
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(IApplicationButtonProvider))]
    internal sealed class EmptyApplicationButtonProvider : IApplicationButtonProvider
    {
        [NotNull] 
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly IUIExtensionService uiExtensionService;

        [ImportingConstructor]
        public EmptyApplicationButtonProvider([NotNull] IEventAggregator eventAggregator, [NotNull] IUIExtensionService uiExtensionService)
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

        public Boolean CanCreateApplicationButton(Application application)
        {
            return application == null;
        }

        public ApplicationButton CreateApplicationButton(Application application)
        {
            var viewModel = new EmptyApplicationViewModel(this.eventAggregator, this.uiExtensionService);

            return new EmptyApplicationButton(viewModel);
        }
    }
}
