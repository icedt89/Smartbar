namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.Localization;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Localization;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    [Export(typeof(ICreateApplicationUICommandProvider))]
    internal sealed class SystemToolsUICommandProvider : ICreateApplicationUICommandProvider
    {
        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly IEnumerable<ISystemToolMetadata> managementConsoleMetadata;

        [NotNull]
        private readonly ILocalizationService localizationService;

        [ImportingConstructor]
        public SystemToolsUICommandProvider([NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull, ImportMany(typeof (ISystemToolMetadata))] IEnumerable<ISystemToolMetadata> managementConsoleMetadata,
            [NotNull] ILocalizationService localizationService)
        {
            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (managementConsoleMetadata == null)
            {
                throw new ArgumentNullException(nameof(managementConsoleMetadata));
            }

            if (localizationService == null)
            {
                throw new ArgumentNullException(nameof(localizationService));
            }

            this.commandDispatcher = commandDispatcher;
            this.managementConsoleMetadata = managementConsoleMetadata;
            this.localizationService = localizationService;
        }

        public IDynamicUICommand CreateUICommand(Guid groupId, Int32 column, Int32 row, Func<Boolean> canExecute)
        {
            var rootMenuItem = new SystemToolsUICommand(() => this.localizationService.Localize<MenuItems>(nameof(MenuItems.CreateSystemToolText)));
            foreach (var processableManagementConsole in this.managementConsoleMetadata.Where(mcm => mcm.IsAvailable).OrderBy(mcm => mcm.GetType().Name))
            {
                if (processableManagementConsole.ShouldRunPostConfigureForSystemTool)
                {
                    processableManagementConsole.PostConfigureSystemTool();    
                }

                var menuItem = new CreateSystemToolUICommand(() => this.localizationService.Localize<DisplayTexts>(processableManagementConsole.DisplayTextResourceName), this.commandDispatcher, processableManagementConsole, groupId, column, row);
                rootMenuItem.AddChildMenuItem(menuItem);
            }

            return rootMenuItem;
        }
    }
}