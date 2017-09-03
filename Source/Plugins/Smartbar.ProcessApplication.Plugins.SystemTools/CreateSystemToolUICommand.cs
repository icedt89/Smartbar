namespace JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools
{
    using System;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Plugins.SystemTools.Metadata;
    using JetBrains.Annotations;

    internal sealed class CreateSystemToolUICommand : DynamicUICommand
    {
        public CreateSystemToolUICommand([NotNull] Func<String> displayTextFactory, [NotNull] ICommandDispatcher commandDispatcher, ISystemToolMetadata systemToolMetadata, Guid groupId, Int32 column, Int32 row) 
            : base(displayTextFactory,
            async () =>
            {
                var applicationId = Guid.NewGuid();

                var createProcessApplicationContainerCommand = new CreateProcessApplicationContainerCommand
                {
                    TargetColumn = column,
                    TargetGroupId = groupId,
                    TargetRow = row,
                    ApplicationCreateTargetBehavior = ApplicationCreateTargetBehavior.OverrideTarget
                };
                createProcessApplicationContainerCommand.Add(new CreateProcessApplicationCommand(applicationId, systemToolMetadata.File, displayTextFactory()));
                createProcessApplicationContainerCommand.Add(new UpdateApplicationWithImageIconApplicationImageCommand(applicationId, systemToolMetadata.IconFile, systemToolMetadata.IconIdentifier, systemToolMetadata.IconIdentifierType));

                await commandDispatcher.DispatchAsync(new[] { createProcessApplicationContainerCommand });
            }, () => systemToolMetadata.IsAvailable)
        {
        }
    }
}
