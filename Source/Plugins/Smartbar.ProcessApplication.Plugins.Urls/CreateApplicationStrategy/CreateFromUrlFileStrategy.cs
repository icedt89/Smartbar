namespace JanHafner.Smartbar.ProcessApplication.Plugins.Urls.CreateApplicationStrategy
{
    using System;
    using System.Diagnostics;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Plugins.Urls.Properties;
    using JanHafner.Toolkit.Common.Ini;
    using JanHafner.Toolkit.Windows;
    using JanHafner.Toolkit.Windows.HotKey;

    internal sealed class CreateFromUrlFileStrategy : ICreateApplicationStrategy
    {
        public CreateApplicationContainerCommand CreateContainerCommand(String data)
        {
            if (String.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            var internetShortcut = new InternetShortcut();
            IniHelper.ReadIniSection(internetShortcut, data);

			if (!Uri.TryCreate(internetShortcut.Url, UriKind.Absolute, out Uri uri))
			{
				throw new InvalidOperationException(String.Format(ExceptionMessages.NoValidUrlSuppliedExceptionMessage, data));
			}

			var defaultBrowser = DefaultBrowser.Current;

            var iconIdentifier = internetShortcut.IconIndex;
            var iconFile = internetShortcut.IconFile;
            var iconIdentifierType = IconIdentifierType.Index;
            if (iconFile == String.Empty && !iconIdentifier.HasValue)
            {
                iconFile = defaultBrowser.IconFile;
                iconIdentifier = defaultBrowser.Identifier;
                iconIdentifierType = defaultBrowser.IconIdentifierType;
            }

            var windowStyle = ProcessWindowStyle.Normal;
            if (internetShortcut.ShowCommand.HasValue)
            {
                if (internetShortcut.ShowCommand == 7)
                {
                    windowStyle = ProcessWindowStyle.Minimized;
                }
                else if (internetShortcut.ShowCommand == 3)
                {
                    windowStyle = ProcessWindowStyle.Maximized;
                }
            }

            var applicationId = Guid.NewGuid();
            var resultingCommands = new CreateProcessApplicationContainerCommand
            {
                new CreateProcessApplicationCommand(applicationId, defaultBrowser.ExePath, internetShortcut.Url),
                new UpdateProcessApplicationCommand(applicationId, defaultBrowser.ExePath, internetShortcut.WorkingDirectory, String.Empty, ProcessPriorityClass.Normal, false, windowStyle),
                new UpdateApplicationWithImageIconApplicationImageCommand(applicationId, iconFile, iconIdentifier.Value, iconIdentifierType)
            };

            if (internetShortcut.HotKey.HasValue)
            {
                var hotKeyModifier = HotKeyModifier.None;
                var hotKey = Key.None;
                if (SystemHotKeyLookupTable.TryGetHotKey(internetShortcut.HotKey.Value, out hotKeyModifier, out hotKey))
                {
                    resultingCommands.Add(new UpdateProcessApplicationHotKeyCommand(applicationId, hotKeyModifier, hotKey));
                }
            }

            return resultingCommands;
        }
    }
}