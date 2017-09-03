namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using System.ComponentModel.Composition;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandHandler))]
    public sealed class CreateProcessApplicationCommandHandler : CreateApplicationCommandHandler
    {
        [ImportingConstructor]
        public CreateProcessApplicationCommandHandler([NotNull] IEventAggregator eventAggregator, [NotNull] ISmartbarDbContext smartbarDbContext) : base(eventAggregator, smartbarDbContext)
        {
        }

        protected override Boolean CanHandleConcreteCommand(CreateApplicationContainerCommand createApplicationsCommand)
        {
            return createApplicationsCommand is CreateProcessApplicationContainerCommand;
        }

        protected override Application CreateNewApplication(CreateApplicationContainerCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var createProcessApplicationContainerCommand = (CreateProcessApplicationContainerCommand)command;

            // Create application.
            var createProcessApplicationCommand = createProcessApplicationContainerCommand.CreateProcessApplicationCommand;
            var processApplication = new ProcessApplication(createProcessApplicationCommand.ApplicationId,
                createProcessApplicationCommand.Execute, createProcessApplicationCommand.Name);

            // Reposition application on desired position inside the assigned group.
            processApplication.Reposition(command.TargetColumn, command.TargetRow);

            // Apply ProcessApplication specific properties.
            foreach (var updateProcessApplicationCommand in createProcessApplicationContainerCommand.UpdateProcessApplicationCommands)
            {
                processApplication.Update(updateProcessApplicationCommand.Execute,
                    updateProcessApplicationCommand.WorkingDirectory, updateProcessApplicationCommand.Arguments,
                    updateProcessApplicationCommand.Priority, null, false, updateProcessApplicationCommand.WindowStyle);
            }

            // Apply ApplicationImage to ProcessApplication.
            foreach (var updateIconApplicationImageCommand in createProcessApplicationContainerCommand.UpdateProcessApplicationImageCommands)
            {
                var applicationImage = this.CreateApplicationImageInstance(updateIconApplicationImageCommand);
                processApplication.UpdateImage(applicationImage);
            }

            // Apply ImpersonationInfo to ProcessApplication.
            foreach (var updateImpersonationCommand in createProcessApplicationContainerCommand.UpdateImpersonationCommands)
            {
                processApplication.UpdateImpersonation(updateImpersonationCommand.Username, updateImpersonationCommand.Password);
            }

            // Apply HotKey to ProcessApplication.
            foreach (var updateHotKeyCommand in createProcessApplicationContainerCommand.UpdateHotKeyCommands)
            {
                processApplication.UpdateHotKey(updateHotKeyCommand.HotKeyModifier, updateHotKeyCommand.HotKey);
            }

            // Apply ProcessAffinityMask to ProcessApplication.
            foreach (var setProcessAffinityMask in createProcessApplicationContainerCommand.SetProcessAffinityMaskCommands)
            {
                processApplication.Update(processApplication.Execute, processApplication.WorkingDirectory, processApplication.Arguments, processApplication.Priority, setProcessAffinityMask.ProcessAffinityMask, processApplication.StretchSmallImage, processApplication.WindowStyle);
            }

            return processApplication;
        }

        private ApplicationImage CreateApplicationImageInstance(IUpdateApplicationImageCommand updateApplicationImageCommand)
        {
            var updateApplicationWithImageIconPackApplicationImageCommand = updateApplicationImageCommand as UpdateApplicationWithImageIconPackApplicationImageCommand;
            if (updateApplicationWithImageIconPackApplicationImageCommand != null)
            {
                return new IconPackApplicationImage(updateApplicationWithImageIconPackApplicationImageCommand.IconPackType, updateApplicationWithImageIconPackApplicationImageCommand.FillColor, updateApplicationWithImageIconPackApplicationImageCommand.IconPackKindKey);
            }

            var updateApplicationWithImageFileApplicationImageCommand = updateApplicationImageCommand as UpdateApplicationWithImageFileApplicationImageCommand;
            if (updateApplicationWithImageFileApplicationImageCommand != null)
            {
                return new FileApplicationImage(updateApplicationWithImageFileApplicationImageCommand.File);
            }

            var updateApplicationWithImageIconApplicationImageCommand = updateApplicationImageCommand as UpdateApplicationWithImageIconApplicationImageCommand;
            if (updateApplicationWithImageIconApplicationImageCommand != null)
            {
                return new IconApplicationImage(updateApplicationWithImageIconApplicationImageCommand.File,
                    updateApplicationWithImageIconApplicationImageCommand.Identifier,
                    updateApplicationWithImageIconApplicationImageCommand.IdentifierType);
            }

            throw new NotSupportedException();
        }
    }
}
