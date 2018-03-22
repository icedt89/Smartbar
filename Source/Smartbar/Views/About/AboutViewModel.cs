namespace JanHafner.Smartbar.Views.About
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using global::Smartbar.Updater.Core;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Mvvm;

    internal sealed class AboutViewModel : BindableBase
    {
        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly ISmartbarUpdater smartbarUpdater;

        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [CanBeNull]
        private Update currentUpdate;

        private Boolean isCheckingForUpdate;

        public AboutViewModel([NotNull] IWindowService windowService, [NotNull] ISmartbarUpdater smartbarUpdater,
            [NotNull] ISmartbarSettings smartbarSettings)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (smartbarUpdater == null)
            {
                throw new ArgumentNullException(nameof(smartbarUpdater));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            this.windowService = windowService;
            this.smartbarUpdater = smartbarUpdater;
            this.smartbarSettings = smartbarSettings;

            var currentAssem = Assembly.GetExecutingAssembly();
            var assemblyCopyrightAttribute = currentAssem.GetCustomAttribute<AssemblyCopyrightAttribute>();
            if (assemblyCopyrightAttribute != null)
            {
                this.ApplicationCopyright = assemblyCopyrightAttribute.Copyright;
            }

            var assemblyProductAttribute = currentAssem.GetCustomAttribute<AssemblyProductAttribute>();
            if (assemblyProductAttribute != null)
            {
                this.ApplicationTitle = assemblyProductAttribute.Product;
            }
        }

        [NotNull]
        public String ApplicationTitle { get; private set; }

        [NotNull]
        public String ApplicationCopyright { get; private set; }

        [NotNull]
        public String ApplicationVersion
        {
            get
            {
                if (this.currentUpdate?.Local == null)
                {
                    return String.Empty;
                }

                return this.currentUpdate.Local.Version.ToString();
            }
        }

        public Boolean IsCheckingForUpdate
        {
            get
            {
                return this.isCheckingForUpdate;
            }
            private set
            {
                if (this.SetProperty(ref this.isCheckingForUpdate, value))
                {
                    this.OnPropertyChanged(() => this.UpdateStatus);
                }
            }
        }

        public String UpdateStatus
        {
            get
            {
                if (this.isCheckingForUpdate)
                {
                    return Localization.About.CheckForUpdateButtonContent;
                }

	            if (this.currentUpdate != null)
	            {
		            if (this.currentUpdate.Remote != null)
		            {
			            return Localization.About.UpdateToVersionButtonContent;
					}

		            if (this.currentUpdate.CheckSuccessful)
		            {
			            return Localization.About.NoUpdateAvailableButtonContent;
					}
	            }

	            return Localization.About.UpdateCheckFailedButtonContent;
			}
		}

        [NotNull]
        public String ApplicationWebsite
        {
            get { return "www.jan-hafner.de"; }
        }

        [NotNull]
        public ICommand CloseCommand
        {
            get { return new CommonOKCommand<AboutViewModel>(this, this.windowService); }
        }

        public async Task CheckForUpdateAsync()
        {
            this.IsCheckingForUpdate = true;

            this.currentUpdate = await this.smartbarUpdater.GetUpdateAsync();

            this.OnPropertyChanged(() => this.ApplicationVersion);

            this.IsCheckingForUpdate = false;
        }

        [NotNull]
        public ICommand StartApplicationUpdate
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Process.Start(this.smartbarSettings.SmartbarUpdaterExePath);
                    Application.Current.Shutdown(0);
                }, () => File.Exists(this.smartbarSettings.SmartbarUpdaterExePath) && this.currentUpdate?.Remote != null);
            }
        }

        [NotNull]
        public ICommand OpenWebsiteCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    Process.Start(this.ApplicationWebsite);
                });
            }
        }
    }
}