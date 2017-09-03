namespace JanHafner.Smartbar.Extensibility.Commanding
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class CompositeCommand<TCommand> : ICompositeCommand<TCommand>
        where TCommand : ICommand
    {
        private readonly IList<TCommand> commands;

        public CompositeCommand()
        {
            this.commands = new List<TCommand>();
        }

        public void Add([NotNull] TCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            this.commands.Add(command);
        }

        public IEnumerator<TCommand> GetEnumerator()
        {
            return this.commands.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class CompositeCommand : CompositeCommand<ICommand>
    {
    }
}
