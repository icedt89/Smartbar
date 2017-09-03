namespace JanHafner.Smartbar.Infrastructure.CommandLine
{
    using System;
    using System.ComponentModel.Composition;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JetBrains.Annotations;

    [Export(typeof(CommandLineHandler))]
    internal sealed class CommandLineHandler
    {
        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [ImportingConstructor]
        public CommandLineHandler([NotNull] ISmartbarSettings smartbarSettings)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            this.smartbarSettings = smartbarSettings;
        }

        public void HandleCommandLine([NotNull] String[] arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            var commandLineProcessor = new CommandLineParser(arguments);

            if (commandLineProcessor.HasVerb(CommandLineVerbs.ModuleExplorerVerb))
            {
                this.HandleModuleExplorerVerb();
            }

            if (commandLineProcessor.HasVerb(CommandLineVerbs.ResetPositionVerb))
            {
                this.HandleResetPositionVerb();
            }
        }

        private void HandleModuleExplorerVerb()
        {
            this.smartbarSettings.IsModuleExplorerAvailable = true;
        }

        private void HandleResetPositionVerb()
        {
            this.smartbarSettings.InitialPosition = new Point(0, 0);
        }
    }
}
