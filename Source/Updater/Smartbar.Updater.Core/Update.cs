namespace Smartbar.Updater.Core
{
    using System;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using NuGet;

    public sealed class Update
    {
        [CanBeNull]
        public LocalVersion Local { get; private set; }

        [CanBeNull]
        public RemoteVersion Remote { get; private set; }

	    public Boolean CheckSuccessful { get; private set; } = true;

		[CanBeNull]
		public Exception UpdateCheckExcetion { get; private set; }

        public void StartSmartbar()
        {
            if (this.Local == null)
            {
                throw new InvalidOperationException();
            }

            Process.Start(this.Local.AssumedSmartbarExeFile);
        }

        public async Task InstallAsync()
        {
            if (this.Local == null || this.Remote == null)
            {
                throw new InvalidOperationException();
            }

            var fileSystem = new PhysicalFileSystem(this.Local.AssumedSmartbarDirectory);
            await Task.Run(() => this.Remote.UpdatePackage.ExtractContents(fileSystem, "."));
        }

	    internal void CheckFailed([CanBeNull] Exception exception)
	    {
		    this.CheckSuccessful = false;
		    this.UpdateCheckExcetion = exception;
	    }

        internal void LocalAvailable([NotNull] LocalVersion local)
        {
            if (local == null)
            {
                throw new ArgumentNullException(nameof(local));
            }

            this.Local = local;
        }

        internal void RemoteFound([NotNull] RemoteVersion remote)
        {
            if (remote == null)
            {
                throw new ArgumentNullException(nameof(remote));
            }

            this.Remote = remote;
        }
    }
}