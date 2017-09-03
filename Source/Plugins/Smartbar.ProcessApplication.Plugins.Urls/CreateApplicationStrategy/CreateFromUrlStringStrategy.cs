namespace JanHafner.Smartbar.ProcessApplication.Plugins.Urls.CreateApplicationStrategy
{
    using System;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Plugins.Urls.Properties;
    using JanHafner.Toolkit.Windows;

    internal sealed class CreateFromUrlStringStrategy : ICreateApplicationStrategy
    {
        public CreateApplicationContainerCommand CreateContainerCommand(String data)
        {
            if (String.IsNullOrWhiteSpace(data))
            {
                throw new ArgumentNullException(nameof(data));
            }

            Uri uri;
            if (!data.TryCreateValidUri(out uri))
            {
                throw new InvalidOperationException(String.Format(ExceptionMessages.NoValidUrlSuppliedExceptionMessage, data));
            }

            var defaultBrowser = DefaultBrowser.Current;

            var applicationId = Guid.NewGuid();
            return new CreateProcessApplicationContainerCommand
            {
                new CreateProcessApplicationCommand(applicationId, defaultBrowser.ExePath, uri.AbsoluteUri),
                new UpdateApplicationWithImageIconApplicationImageCommand(applicationId, defaultBrowser.IconFile, defaultBrowser.Identifier.Value, defaultBrowser.IconIdentifierType)
            };
        }
    }
}