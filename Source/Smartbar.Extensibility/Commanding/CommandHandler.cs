namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Prism.Events;

    public abstract class CommandHandler<TCommand> : ICommandHandler
        where TCommand : ICommand
    {
        [NotNull]
        protected readonly IEventAggregator EventAggregator;

        protected CommandHandler([NotNull] IEventAggregator eventAggregator)
        {
            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.EventAggregator = eventAggregator;
        }

        public abstract Task HandleAsync([NotNull] TCommand command);

        public virtual async Task HandleAsync(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (!(await this.CanHandleAsync(command)))
            {
                throw new ArgumentException();
            }

            await this.HandleAsync((TCommand)command);
        }

        public virtual Task<Boolean> CanHandleAsync(ICommand command)
        {
            return Task.FromResult(command is TCommand);
        }

        protected void PublishCommandHandlerDone([NotNull] TCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            this.EventAggregator.GetEvent<CommandHandlerDone>().Publish(new CommandHandlerDone.Data(this, command));
        }
    }
}
