namespace JanHafner.Smartbar.Common.UserInterface.BuiltIn
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Common.UserInterface.Localization;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    [Export(typeof(IImageVisualizationUICommandProvider))]
    internal sealed class ImageFileVisualizationUICommandProvider : IImageVisualizationUICommandProvider
    {
        [NotNull]
        private readonly ISmartbarService smartbarService;

        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly ILocalizationService localizationService;

        [ImportingConstructor]
        public ImageFileVisualizationUICommandProvider([NotNull] ISmartbarService smartbarService,
            [NotNull] IWindowService windowService,
            [NotNull] ICommandDispatcher commandDispatcher, [NotNull] ILocalizationService localizationService)
        {
            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            this.smartbarService = smartbarService;
            this.windowService = windowService;
            this.commandDispatcher = commandDispatcher;
            this.localizationService = localizationService;
        }

        public IDynamicUICommand CreateUICommand(Guid applicationId, Func<Boolean> canExecute)
        {
            if (canExecute == null)
            {
                throw new ArgumentNullException(nameof(canExecute));
            }

            return new ImageFileVisualizationUICommand(() => this.localizationService.Localize<MenuItems>(nameof(MenuItems.ProcessApplicationButtonChangeDisplayImageFromImageFile)), this.smartbarService, this.windowService, this.commandDispatcher, applicationId, canExecute);
        }
    }
}