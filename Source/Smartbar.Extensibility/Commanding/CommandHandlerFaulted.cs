namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;
    using JetBrains.Annotations;
    using Prism.Events;

    public sealed class CommandHandlerFaulted : PubSubEvent<CommandHandlerFaulted.Data>
    {
        public sealed class Data
        {
            public Data([NotNull] ICommandHandler commandHandler, [NotNull] ICommand command,
                [NotNull] Exception exception)
            {
                if (commandHandler == null)
                {
                    throw new ArgumentNullException(nameof(commandHandler));
                }

                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }

                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                this.CommandHandler = commandHandler;
                this.Command = command;
                this.Exception = exception;
            }

            [NotNull]
            public ICommandHandler CommandHandler { get; private set; }

            [NotNull]
            public ICommand Command { get; private set; }

            [NotNull]
            public Exception Exception { get; private set; }

            public Boolean Handled { get; set; }
        }
    }
}
