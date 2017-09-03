namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    public sealed class RenameProcessApplicationCommand : ICommand
    {
        public RenameProcessApplicationCommand(Guid applicationId, [NotNull] String name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));    
            }

            this.ApplicationId = applicationId;
            this.Name = name;
        }
        
        public Guid ApplicationId { get; private set; }

        [NotNull]
        public String Name { get; private set; }
    }
}
