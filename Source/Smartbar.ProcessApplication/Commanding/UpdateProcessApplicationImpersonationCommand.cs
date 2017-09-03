namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using System.Security;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    public sealed class UpdateImpersonationCommand : ICommand
    {
        public UpdateImpersonationCommand(Guid applicationId, [CanBeNull] String username, [CanBeNull] SecureString password)
        {
            this.ApplicationId = applicationId;
            this.Username = username;
            this.Password = password;
        }

        public Guid ApplicationId { get; private set; }

        [CanBeNull]
        public String Username { get; private set; }

        [CanBeNull]
        public SecureString Password { get; private set; }
    }
}
