namespace JanHafner.Smartbar.Infrastructure.Commanding
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandDispatcher))]
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        [NotNull]
        private readonly IEnumerable<ICommandHandler> commandHandler;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [ImportingConstructor]
        public CommandDispatcher([NotNull] [ImportMany(typeof(ICommandHandler))] IEnumerable<ICommandHandler> commandHandler,
            [NotNull] IEventAggregator eventAggregator)
        {
            if (commandHandler == null)
            {
                throw new ArgumentNullException(nameof(commandHandler));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.commandHandler = commandHandler;
            this.eventAggregator = eventAggregator;
        }

        public async Task DispatchAsync(IEnumerable<ICommand> commands)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }

            var processableCommands = commands.ToList();
            foreach (var command in processableCommands)
            {
                var handler = await this.commandHandler.Overridden().FirstAsync(m => m.CanHandleAsync(command));
                try
                {
                    await handler.HandleAsync(command);
                }
                catch (Exception ex)
                {
                    var data = new CommandHandlerFaulted.Data(handler, command, ex);
                    this.eventAggregator.GetEvent<CommandHandlerFaulted>().Publish(data);

                    if (!data.Handled)
                    {
                        throw;
                    }
                }
            }

            this.eventAggregator.GetEvent<CommandProcessorSuccessfullyCompleted>().Publish(processableCommands);
        }
    }
}
