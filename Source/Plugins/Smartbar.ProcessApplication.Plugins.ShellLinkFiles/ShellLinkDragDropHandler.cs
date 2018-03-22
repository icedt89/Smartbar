namespace JanHafner.Smartbar.ProcessApplication.Plugins.ShellLinkFiles
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Windows.HotKey;
    using JetBrains.Annotations;
    using Microsoft.WindowsAPICodePack.Shell;
    using Prism.Events;

    [HandlerOverride(typeof(ProcessApplicationDragDropHandler))]
    [Export(typeof(IApplicationCreationHandler))]
    public class ShellLinkDragDropHandler : IApplicationCreationHandler, IDisposable
    {
        [NotNull]
        public readonly String LinkFileExtension = ".lnk";

        internal const String PluginName = "Shell Link Drag & Drop";

        [UsedImplicitly]
        [NotNull]
        [ImportMany(typeof(IApplicationCreationHandler))]
        private IEnumerable<IApplicationCreationHandler> dropHandler;

        [NotNull]
        private readonly IPluginConfigurationService pluginConfigurationService;

        [NotNull]
        private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull]
        private readonly IDictionary<Guid, String> handledFiles;

        [ImportingConstructor]
        public ShellLinkDragDropHandler([NotNull] IPluginConfigurationService pluginConfigurationService,
            [NotNull] IEventAggregator eventAggregator)
        {
            if (pluginConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(pluginConfigurationService));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.handledFiles = new Dictionary<Guid, String>();
            this.pluginConfigurationService = pluginConfigurationService;
            this.subscriptionTokens = new List<SubscriptionToken>();

            this.InitializeSubscriptions(eventAggregator);
        }

        private void InitializeSubscriptions(IEventAggregator eventAggregator)
        {
            this.subscriptionTokens.Add(eventAggregator.GetEvent<ApplicationsCreated>().Subscribe(data =>
            {
                var pluginConfiguration =
                    this.pluginConfigurationService
                        .GetConfigurationOrDefault<ShellLinkDragDropHandlerPluginConfiguration>();
                foreach (var application in data.Applications)
                {
					if (!this.handledFiles.TryGetValue(application.Id, out string file))
					{
						continue;
					}

					// Remove the entry from the dictionary in any case.
					this.handledFiles.Remove(application.Id);

                    if (!pluginConfiguration.TryDeleteSourceShellLink)
                    {
                        continue;
                    }

                    try
                    {
                        File.Delete(file);
                    }
                    catch (IOException ioException)
                    {
                        Debug.Assert(false, "Loggen!" + ioException);
                    }
                }
            }, ThreadOption.BackgroundThread, true, data => this.handledFiles.Keys.Intersect(data.Applications.Select(a => a.Id)).Any()));
        }

        public void Dispose()
        {
            this.subscriptionTokens.UnsubscribeAll();
        }

        public Boolean CanCreate(Object data)
        {
            var stringData = data as String;
            if (stringData == null)
            {
                return false;
            }
            
            var canCreateFromFile = ShellObject.IsPlatformSupported && File.Exists(stringData) && Path.GetExtension(stringData).Equals(this.LinkFileExtension, StringComparison.CurrentCultureIgnoreCase);
            if (!canCreateFromFile)
            {
                return false;
            }

            var shellObject = ShellObject.FromParsingName(stringData);
            return shellObject.IsLink && shellObject.IsFileSystemObject;
        }

        public ICreateApplicationCommand CreateCommand(Object data)
        {
            var stringData = (String)data;

            var shellFile = (ShellFile)ShellObject.FromParsingName(stringData);
            var targetPath = shellFile.Properties.System.Link.TargetParsingPath.Value;

            var dropHandlers = this.dropHandler.Where(dh => dh.CanCreate(targetPath));
            var foundDropHandler = dropHandlers.Overridden(this).Single();

            var result = foundDropHandler.CreateCommand(targetPath);

            var createProcessApplicationContainerCommand = (CreateProcessApplicationContainerCommand)result;
            var createProcessApplicationCommand = (CreateProcessApplicationCommand)createProcessApplicationContainerCommand.Single(command => command is CreateProcessApplicationCommand);

            var hotKeyModifier = HotKeyModifier.None;
            var hotKey = Key.None;
            if (ShellLinkHotKeyExtractor.TryGetHotKey(shellFile, out hotKeyModifier, out hotKey))
            {
                createProcessApplicationContainerCommand.Add(new UpdateProcessApplicationHotKeyCommand(createProcessApplicationCommand.ApplicationId, hotKeyModifier, hotKey));
            }

            this.handledFiles.Add(createProcessApplicationCommand.ApplicationId, stringData);

            return result;
        }
    }
}