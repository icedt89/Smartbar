namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata
{
    using System;
    using JanHafner.Toolkit.Windows;

    internal interface ISystemToolMetadata
    {
        String DisplayTextResourceName { get; }

        String File { get; }

        String IconFile { get; }

        Int32 IconIdentifier { get; }

        IconIdentifierType IconIdentifierType { get; }

        Boolean IsAvailable { get; }

        Boolean ShouldRunPostConfigureForSystemTool { get; }

        void PostConfigureSystemTool();
    }
}