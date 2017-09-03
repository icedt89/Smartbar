namespace JanHafner.Smartbar.Infrastructure.CommandLine
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;

    internal sealed class CommandLineParser
    {
        private readonly String[] arguments;

        public CommandLineParser([NotNull] String[] arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            this.arguments = arguments;
        }

        public Boolean HasVerb(String verb)
        {
            return this.arguments.Contains(verb);
        }
    }
}
