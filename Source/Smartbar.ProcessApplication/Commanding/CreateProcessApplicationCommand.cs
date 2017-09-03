namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    public sealed class CreateProcessApplicationCommand : ICommand
    {
        public CreateProcessApplicationCommand(Guid applicationId, [NotNull] String execute, [NotNull] String name)
        {
            if (String.IsNullOrWhiteSpace(execute))
            {
                throw new ArgumentNullException(nameof(execute));
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.ApplicationId = applicationId;
            this.Execute = execute;
            this.Name = name;
        }

        public Guid ApplicationId { get; private set; }

        [NotNull]
        public String Execute { get; private set; }

        [NotNull]
        public String Name { get; private set; }
    }
}
