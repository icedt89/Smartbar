namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using System.Diagnostics;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    public sealed class UpdateProcessApplicationCommand : ICommand
    {
        public UpdateProcessApplicationCommand(Guid applicationId, [NotNull] String execute, [CanBeNull] String workingDirectory, [CanBeNull] String arguments, ProcessPriorityClass priority, Boolean stretchSmallImage, ProcessWindowStyle windowStyle)
        {
            if (String.IsNullOrWhiteSpace(execute))
            {
                throw new ArgumentNullException(nameof(execute));
            }

            this.ApplicationId = applicationId;
            this.Execute = execute;
            this.WorkingDirectory = workingDirectory;
            this.Arguments = arguments;
            this.Priority = priority;
            this.WindowStyle = windowStyle;
            this.StretchSmallImage = stretchSmallImage;
        }

        public Guid ApplicationId { get; private set; }

        public ProcessWindowStyle WindowStyle { get; private set; }

        [NotNull]
        public String Execute { get; private set; }

        [CanBeNull]
        public String WorkingDirectory { get; private set; }

        [CanBeNull]
        public String Arguments { get; private set; }

        public ProcessPriorityClass Priority { get; private set; }

        public Boolean StretchSmallImage { get; private set; }
    }
}
