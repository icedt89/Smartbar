namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    public abstract class CreateApplicationCommandHandler : ICommandHandler
    {
        [NotNull]
        protected readonly IEventAggregator EventAggregator;

        [NotNull]
        protected readonly ISmartbarDbContext SmartbarDbContext;

        protected CreateApplicationCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull] ISmartbarDbContext smartbarDbContext)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.EventAggregator = eventAggregator;
            this.SmartbarDbContext = smartbarDbContext;
        }

        public virtual async Task<Boolean> CanHandleAsync(ICommand command)
        {
            await Task.Yield();

            var createApplicationsCommand = command as CreateApplicationsCommand;
            if (createApplicationsCommand != null)
            {
                foreach (var unknownCommand in createApplicationsCommand)
                {
                    if (this.CanHandleConcreteCommand(unknownCommand))
                    {
                        return true;
                    }
                }
            }

            return this.CanHandleConcreteCommand(command);
        }

        protected abstract Boolean CanHandleConcreteCommand(CreateApplicationContainerCommand createApplicationsCommand);

        private Boolean CanHandleConcreteCommand(ICommand unknownCommand)
        {
            return unknownCommand is MoveApplicationCommand || unknownCommand is ExchangeApplicationCommand || unknownCommand is CopyApplicationCommand || (unknownCommand is CreateApplicationContainerCommand && this.CanHandleConcreteCommand((CreateApplicationContainerCommand) unknownCommand));
        }

        public async Task HandleAsync(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var commands = new List<ICommand>();
            if (command is CreateApplicationsCommand)
            {
                commands.AddRange((CreateApplicationsCommand)command);
            }
            else
            {
                commands.Add(command);
            }

            var eventPublishingActions = new List<Action>();
            foreach (var unknownCommand in this.CaptureApplications(commands))
            {
                eventPublishingActions.AddRange(this.HandleConcreteCommand(unknownCommand));
            }

            if (!eventPublishingActions.Any())
            {
                return;
            }

            await this.SmartbarDbContext.SaveChangesAsync();

            foreach (var eventPublishingAction in eventPublishingActions)
            {
                eventPublishingAction();
            }

            this.EventAggregator.GetEvent<CommandHandlerDone>().Publish(new CommandHandlerDone.Data(this, command));
        }

        [NotNull]
        private IEnumerable<Action> HandleConcreteCommand(
            [NotNull] ICommand unknownCommand)
        {
            if (unknownCommand == null)
            {
                throw new ArgumentNullException(nameof(unknownCommand));
            }

            var capturedInternalCreateApplicationInfoCommand = unknownCommand as CapturedInternalCreateApplicationInfoCommand;
            if (capturedInternalCreateApplicationInfoCommand != null)
            {
                if (capturedInternalCreateApplicationInfoCommand.Command is MoveApplicationCommand)
                {
                    return this.HandleMoveApplicationCommand(capturedInternalCreateApplicationInfoCommand);
                }

                if (capturedInternalCreateApplicationInfoCommand.Command is CopyApplicationCommand)
                {
                    return this.HandleCopyCommandCommand(capturedInternalCreateApplicationInfoCommand);
                }

                if (capturedInternalCreateApplicationInfoCommand.Command is ExchangeApplicationCommand)
                {
                    return this.HandleExchangeApplicationCommand(capturedInternalCreateApplicationInfoCommand);
                }
            }

            if (unknownCommand is CreateApplicationContainerCommand)
            {
                return this.HandleCreateApplicationContainerCommand((CreateApplicationContainerCommand) unknownCommand);
            }

            throw new NotSupportedException($"Command type {unknownCommand.GetType().Name} is not supported.");
        }

        private IEnumerable<ICommand> CaptureApplications(
            IEnumerable<ICommand> commands)
        {
            var groups = this.SmartbarDbContext.Groups.ToDictionary(group => group.Id, group => group);
            var applications = this.SmartbarDbContext.Groups.SelectMany(group => group.Applications).ToDictionary(application => application.Id, application => application);

            foreach (var command in commands)
            {
                var internalCreateApplicationCommand = command as InternalCreateApplicationCommand;
                if (internalCreateApplicationCommand != null)
                {
                    var targetApplication = applications.Values.SingleOrDefault(application => application.Row == internalCreateApplicationCommand.TargetRow && application.Column == internalCreateApplicationCommand.TargetColumn);

                    yield return new CapturedInternalCreateApplicationInfoCommand(groups[internalCreateApplicationCommand.SourceGroupId], applications[internalCreateApplicationCommand.SourceApplicationId], groups[internalCreateApplicationCommand.TargetGroupId], targetApplication, internalCreateApplicationCommand);
                }
                else
                {
                    yield return command;
                }
            }
        }

        [NotNull]
        private IEnumerable<Action> HandleCreateApplicationContainerCommand(CreateApplicationContainerCommand createApplicationContainerCommand)
        {
            if (createApplicationContainerCommand == null)
            {
                throw new ArgumentNullException(nameof(createApplicationContainerCommand));
            }

            var assignedGroup = this.SmartbarDbContext.Groups.Single(group => group.Id == createApplicationContainerCommand.TargetGroupId);

            var result = new List<Action>();
            var targetApplication = assignedGroup.Applications.SingleOrDefault(a => a.Row == createApplicationContainerCommand.TargetRow && a.Column == createApplicationContainerCommand.TargetColumn);
            if (targetApplication != null)
            {
                if (createApplicationContainerCommand.ApplicationCreateTargetBehavior == ApplicationCreateTargetBehavior.OverrideTarget)
                {
                    assignedGroup.Applications.Remove(targetApplication);
                    result.Add(() => this.EventAggregator.GetEvent<ApplicationsDeleted>().Publish(new ApplicationsDeleted.Data(assignedGroup, targetApplication)));
                }
                else
                {
                    return Enumerable.Empty<Action>();
                }
            }
            
            var application = this.CreateNewApplication(createApplicationContainerCommand);
            application.Reposition(createApplicationContainerCommand.TargetColumn, createApplicationContainerCommand.TargetRow);
            assignedGroup.Applications.Add(application);

            result.Add(() => this.EventAggregator.GetEvent<ApplicationsCreated>().Publish(new ApplicationsCreated.Data(assignedGroup, application)));

            return result;
        }

        [NotNull]
        private IEnumerable<Action> HandleMoveApplicationCommand(CapturedInternalCreateApplicationInfoCommand capturedInternalCreateApplicationInfoCommand)
        {
            if (capturedInternalCreateApplicationInfoCommand.SourceApplication == capturedInternalCreateApplicationInfoCommand.TargetApplication)
            {
                return Enumerable.Empty<Action>();
            }

            var result = new List<Action>();
            if (capturedInternalCreateApplicationInfoCommand.TargetApplication != null)
            {
                if (capturedInternalCreateApplicationInfoCommand.Command.ApplicationCreateTargetBehavior == ApplicationCreateTargetBehavior.OverrideTarget)
                {
                    capturedInternalCreateApplicationInfoCommand.TargetGroup.Applications.Remove(capturedInternalCreateApplicationInfoCommand.TargetApplication);
                    result.Add(() => this.EventAggregator.GetEvent<ApplicationsDeleted>().Publish(new ApplicationsDeleted.Data(capturedInternalCreateApplicationInfoCommand.TargetGroup, capturedInternalCreateApplicationInfoCommand.TargetApplication)));
                }
                else
                {
                    return Enumerable.Empty<Action>();
                }
            }

            capturedInternalCreateApplicationInfoCommand.SourceGroup.Applications.Remove(capturedInternalCreateApplicationInfoCommand.SourceApplication);
            result.Add(() => this.EventAggregator.GetEvent<ApplicationsDeleted>().Publish(new ApplicationsDeleted.Data(capturedInternalCreateApplicationInfoCommand.SourceGroup, capturedInternalCreateApplicationInfoCommand.SourceApplication)));

            var newSourceApplication = this.EnsureApplicationIsClonable(capturedInternalCreateApplicationInfoCommand.SourceApplication);
            newSourceApplication.Reposition(capturedInternalCreateApplicationInfoCommand.Command.TargetColumn, capturedInternalCreateApplicationInfoCommand.Command.TargetRow);
            capturedInternalCreateApplicationInfoCommand.TargetGroup.Applications.Add(newSourceApplication);
            result.Add(() => this.EventAggregator.GetEvent<ApplicationsCreated>().Publish(new ApplicationsCreated.Data(capturedInternalCreateApplicationInfoCommand.TargetGroup, newSourceApplication)));

            return result;
        }

        [NotNull]
        private IEnumerable<Action> HandleExchangeApplicationCommand(CapturedInternalCreateApplicationInfoCommand capturedInternalCreateApplicationInfoCommand)
        {
            if (capturedInternalCreateApplicationInfoCommand.SourceApplication == capturedInternalCreateApplicationInfoCommand.TargetApplication)
            {
                return Enumerable.Empty<Action>();
            }

            var result = new List<Action>();

            capturedInternalCreateApplicationInfoCommand.SourceGroup.Applications.Remove(capturedInternalCreateApplicationInfoCommand.SourceApplication);
            result.Add(() => this.EventAggregator.GetEvent<ApplicationsDeleted>().Publish(new ApplicationsDeleted.Data(capturedInternalCreateApplicationInfoCommand.SourceGroup, capturedInternalCreateApplicationInfoCommand.SourceApplication)));

            if (capturedInternalCreateApplicationInfoCommand.TargetApplication != null)
            {
                capturedInternalCreateApplicationInfoCommand.TargetGroup.Applications.Remove(capturedInternalCreateApplicationInfoCommand.TargetApplication);
                result.Add(() => this.EventAggregator.GetEvent<ApplicationsDeleted>().Publish(new ApplicationsDeleted.Data(capturedInternalCreateApplicationInfoCommand.TargetGroup, capturedInternalCreateApplicationInfoCommand.TargetApplication)));

                capturedInternalCreateApplicationInfoCommand.TargetApplication.Reposition(capturedInternalCreateApplicationInfoCommand.SourceApplication.Column, capturedInternalCreateApplicationInfoCommand.SourceApplication.Row);
                capturedInternalCreateApplicationInfoCommand.SourceGroup.Applications.Add(capturedInternalCreateApplicationInfoCommand.TargetApplication);

                result.Add(() => this.EventAggregator.GetEvent<ApplicationsCreated>().Publish(new ApplicationsCreated.Data(capturedInternalCreateApplicationInfoCommand.SourceGroup, capturedInternalCreateApplicationInfoCommand.TargetApplication)));
            }

            var newSourceApplication = this.EnsureApplicationIsClonable(capturedInternalCreateApplicationInfoCommand.SourceApplication);
            newSourceApplication.Reposition(capturedInternalCreateApplicationInfoCommand.Command.TargetColumn, capturedInternalCreateApplicationInfoCommand.Command.TargetRow);
            capturedInternalCreateApplicationInfoCommand.TargetGroup.Applications.Add(newSourceApplication);

            result.Add(() => this.EventAggregator.GetEvent<ApplicationsCreated>().Publish(new ApplicationsCreated.Data(capturedInternalCreateApplicationInfoCommand.TargetGroup, newSourceApplication)));

            return result;
        }

        [NotNull]
        private IEnumerable<Action> HandleCopyCommandCommand(CapturedInternalCreateApplicationInfoCommand capturedInternalCreateApplicationInfoCommand)
        {
            if (capturedInternalCreateApplicationInfoCommand.SourceApplication == capturedInternalCreateApplicationInfoCommand.TargetApplication)
            {
                return Enumerable.Empty<Action>();
            }

            var result = new List<Action>();
            if (capturedInternalCreateApplicationInfoCommand.TargetApplication != null)
            {
                if (capturedInternalCreateApplicationInfoCommand.Command.ApplicationCreateTargetBehavior == ApplicationCreateTargetBehavior.OverrideTarget)
                {
                    capturedInternalCreateApplicationInfoCommand.TargetGroup.Applications.Remove(capturedInternalCreateApplicationInfoCommand.TargetApplication);
                    result.Add(() => this.EventAggregator.GetEvent<ApplicationsDeleted>().Publish(new ApplicationsDeleted.Data(capturedInternalCreateApplicationInfoCommand.TargetGroup, capturedInternalCreateApplicationInfoCommand.TargetApplication)));
                }
                else
                {
                    return Enumerable.Empty<Action>();
                }
            }

            var clonedSourceApplication = this.EnsureApplicationIsClonable(capturedInternalCreateApplicationInfoCommand.SourceApplication);
            clonedSourceApplication.Reposition(capturedInternalCreateApplicationInfoCommand.Command.TargetColumn, capturedInternalCreateApplicationInfoCommand.Command.TargetRow);

            capturedInternalCreateApplicationInfoCommand.TargetGroup.Applications.Add(clonedSourceApplication);
            result.Add(() => this.EventAggregator.GetEvent<ApplicationsCreated>().Publish(new ApplicationsCreated.Data(capturedInternalCreateApplicationInfoCommand.TargetGroup, clonedSourceApplication)));

            return result;
        }
        
        [NotNull]
        protected abstract Application CreateNewApplication(CreateApplicationContainerCommand command);

        [NotNull]
        private Application EnsureApplicationIsClonable(Application application)
        {
            var clonableSourceApplication = application as ICloneable;
            if (clonableSourceApplication == null)
            {
                throw new NotImplementedException($"The application of type '{application.GetType().Name}' must implement IClonable to get copied.");
            }

            return (Application)clonableSourceApplication.Clone();
        }

        private sealed class CapturedInternalCreateApplicationInfoCommand : ICommand
        {
            public CapturedInternalCreateApplicationInfoCommand([NotNull] Group sourceGroup,
                [NotNull] Application sourceApplication, [NotNull] Group targetGroup,
                [CanBeNull] Application targetApplication,
                [NotNull] InternalCreateApplicationCommand command)
            {
                if (sourceGroup == null)
                {
                    throw new ArgumentNullException(nameof(sourceGroup));
                }

                if (sourceApplication == null)
                {
                    throw new ArgumentNullException(nameof(sourceApplication));
                }

                if (targetGroup == null)
                {
                    throw new ArgumentNullException(nameof(targetGroup));
                }

                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }

                this.SourceGroup = sourceGroup;
                this.SourceApplication = sourceApplication;
                this.TargetGroup = targetGroup;
                this.TargetApplication = targetApplication;
                this.Command = command;
            }

            [NotNull]
            public Group SourceGroup { get; private set; }

            [NotNull]
            public Application SourceApplication { get; private set; }

            [NotNull]
            public Group TargetGroup { get; private set; }

            [CanBeNull]
            public Application TargetApplication { get; private set; }

            [NotNull]
            public InternalCreateApplicationCommand Command { get; private set; }
        }
    }
}