namespace JanHafner.Smartbar.ProcessApplication.ApplicationCreationHandler
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
    using JanHafner.Toolkit.Windows;

    [Export(typeof(IApplicationCreationHandler))]
    public class ProcessApplicationDragDropHandler : IApplicationCreationHandler
    {
        public virtual Boolean CanCreate(Object data)
        {
            var stringData = data as String;
            return stringData != null && File.Exists(stringData);
        }

        public ICreateApplicationCommand CreateCommand(object data)
        {
            var stringData = (String)data;

            var name = PathUtilities.GetIdealFileDisplayName(stringData);

            Int32? identifier;
            var identifierType = IconIdentifierType.Unknown;
            var file = SafeNativeMethods.RetrieveAssociatedIcon(stringData, out identifier, out identifierType);
            if (file == String.Empty && !identifier.HasValue)
            {
                identifier = 0;
                identifierType = IconIdentifierType.Index;
                file = stringData;
            }

            var applicationId = Guid.NewGuid();
            return new CreateProcessApplicationContainerCommand
            {
                new CreateProcessApplicationCommand(applicationId, stringData, name),
                new UpdateProcessApplicationCommand(applicationId, stringData, null, null, ProcessPriorityClass.Normal, false, ProcessWindowStyle.Normal),
                new UpdateApplicationWithImageIconApplicationImageCommand(applicationId, file, identifier.Value, identifierType)
            };
        }
    }
}