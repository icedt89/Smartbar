namespace Smartbar.Updater.Core
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Common;
    using JetBrains.Annotations;
    using NuGet;

    [Export(typeof (ISmartbarUpdater))]
    internal sealed class SmartbarUpdater : ISmartbarUpdater
    {
        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [ImportingConstructor]
        public SmartbarUpdater([NotNull] ISmartbarSettings smartbarSettings)
        {
            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            this.smartbarSettings = smartbarSettings;
        }

        public async Task<Update> GetUpdateAsync()
        {
            var result = new Update();
            try
            {
                String assumedSmartbarDirectory;
                var assumedSmartbarExeFile = this.TryFindSmartbarExe(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), out assumedSmartbarDirectory);
                if (File.Exists(assumedSmartbarExeFile))
                {
                    var fileVersionInfo = FileVersionInfo.GetVersionInfo(assumedSmartbarExeFile);
                    var currentSmartbarVersion = SemanticVersion.Parse(fileVersionInfo.ProductVersion);

                    result.LocalAvailable(new LocalVersion(assumedSmartbarExeFile, currentSmartbarVersion, assumedSmartbarDirectory));
                }

                if (result.Local != null)
                {
                    await this.UpdateRemoteInformationAsync(result);
                }
            }
            catch
            {
            }

            return result;
        }

        public async Task UpdateRemoteInformationAsync(Update update)
        {
            if (update == null)
            {
                throw new ArgumentNullException(nameof(update));
            }

            var updateRepository = PackageRepositoryFactory.Default.CreateRepository(this.smartbarSettings.PluginPackagesFeed);
            var updatePackage = (await Task.Run(() => updateRepository.GetUpdates(
                new[]
                {
                        new PackageName(Properties.Settings.Default.SmartbarPackageId, update.Local.Version)
                },
                false, false))).SingleOrDefault();
            if (updatePackage != null)
            {
                update.RemoteFound(new RemoteVersion(updatePackage.Version, updatePackage));
            }
        }

        [CanBeNull]
        private String TryFindSmartbarExe([NotNull] String firstAttemptedDirectory, [CanBeNull] out String assumedSmartbarExeDirectory, Int32 currentWalkedUpDirectoryCount = 0)
        {
            if (String.IsNullOrWhiteSpace(firstAttemptedDirectory))
            {
                throw new ArgumentNullException(nameof(firstAttemptedDirectory));
            }

            if (currentWalkedUpDirectoryCount > 1)
            {
                assumedSmartbarExeDirectory = String.Empty;
                return String.Empty;
            }

            var assumedSmartbarExeFile = Path.Combine(firstAttemptedDirectory, Properties.Settings.Default.SmartbarExeName);

            if (!File.Exists(assumedSmartbarExeFile))
            {
                return this.TryFindSmartbarExe(Directory.GetParent(firstAttemptedDirectory).FullName, out assumedSmartbarExeDirectory, currentWalkedUpDirectoryCount++);
            }

            assumedSmartbarExeDirectory = firstAttemptedDirectory;
            return assumedSmartbarExeFile;
        }
    }
}