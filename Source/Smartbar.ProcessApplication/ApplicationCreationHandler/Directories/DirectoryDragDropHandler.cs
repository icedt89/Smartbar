namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler.Directories
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;

    [Export(typeof(IApplicationCreationHandler))]
    public class DirectoryDragDropHandler : IApplicationCreationHandler
    {
        internal const String PluginName = "Directory Drag & Drop";

        [NotNull]
        private readonly IPluginConfigurationService pluginConfigurationService;
        
        [ImportingConstructor]
        public DirectoryDragDropHandler([NotNull] IPluginConfigurationService pluginConfigurationService)
        {
            if (pluginConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(pluginConfigurationService));
            }

            this.pluginConfigurationService = pluginConfigurationService;
        }

        private void ApplyDesktopIni([NotNull] ShellClassInfo shellClassInfo, ref String file, ref Int32 identifier, ref IconIdentifierType identifierType, ref String name)
        {
            if (shellClassInfo == null)
            {
                throw new ArgumentNullException(nameof(shellClassInfo));
            }

            NativeResourceDescriptor iconResourceDescriptor;
            if (shellClassInfo.IconIndex.HasValue && !String.IsNullOrWhiteSpace(shellClassInfo.IconFile))
            {
                identifier = shellClassInfo.IconIndex.Value;
                file = Environment.ExpandEnvironmentVariables(shellClassInfo.IconFile);
                identifierType = IconIdentifier.Identify(identifier);
            }
            else if (NativeResourceDescriptor.TryParseFromResourceString(shellClassInfo.IconResource,
                out iconResourceDescriptor))
            {
                identifier = (Int32)iconResourceDescriptor.ResourceId;
                file = iconResourceDescriptor.File;
                identifierType = IconIdentifierType.ResourceId;
            }

            NativeResourceDescriptor localizedResourceDescriptor;
            if (NativeResourceDescriptor.TryParseFromResourceString(shellClassInfo.LocalizedResourceName,
                out localizedResourceDescriptor))
            {
                using (var nativeExecutable = new NativeExecutable(localizedResourceDescriptor.File))
                {
                    var localizedResourceString = nativeExecutable.GetResourceString(localizedResourceDescriptor);
                    if (!String.IsNullOrWhiteSpace(localizedResourceString))
                    {
                        name = localizedResourceString;
                    }
                }
            }
        }

        public Boolean CanCreate(Object data)
        {
            var stringData = data as String;
            return stringData != null && Directory.Exists(stringData);
        }

        public ICreateApplicationCommand CreateCommand(object data)
        {
            var stringData = (String)data;

            var identifier = 0;
            var file = PathUtilities.GetDefaultDirectoryIcon(out identifier);
            var identifierType = IconIdentifierType.Index;

            var name = PathUtilities.GetIdealDirectoryDisplayName(stringData);
            var pluginConfiguration = this.pluginConfigurationService.GetConfigurationOrDefault<DirectoryDragDropHandlerPluginConfiguration>();

            ShellClassInfo shellClassInfo;
            if (pluginConfiguration.ProcessDesktopIni && DesktopIniHelper.TryGetShellClassInfo(stringData, out shellClassInfo))
            {
                this.ApplyDesktopIni(shellClassInfo, ref file, ref identifier, ref identifierType, ref name);
            }

            var explorerPath = PathUtilities.GetExplorer();

            var applicationId = Guid.NewGuid();
            return new CreateProcessApplicationContainerCommand
            {
                new CreateProcessApplicationCommand(applicationId, explorerPath, name),
                new UpdateProcessApplicationCommand(applicationId, explorerPath, null, stringData, ProcessPriorityClass.Normal, false, ProcessWindowStyle.Normal),
                new UpdateApplicationWithImageIconApplicationImageCommand(applicationId, file, identifier, identifierType)
            };
        }
    }
}
