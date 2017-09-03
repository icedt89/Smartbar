namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;
    using JetBrains.Annotations;
    using Prism.Events;

    public sealed class CommandHandlerDone : PubSubEvent<CommandHandlerDone.Data>
    {
        public sealed class Data
        {
            public Data([NotNull] ICommandHandler commandHandler, [NotNull] ICommand command)
            {
                if (commandHandler == null)
                {
                    throw new ArgumentNullException(nameof(commandHandler));
                }

                if (command == null)
                {
                    throw new ArgumentNullException(nameof(command));
                }

                this.CommandHandler = commandHandler;
                this.Command = command;
            }

            [NotNull]
            public ICommandHandler CommandHandler { get; private set; }

            [NotNull]
            public ICommand Command { get; private set; }
        }
    }
}
