namespace JanHafner.Smartbar.ProcessApplication.ProcessApplicationButton
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Localization;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Events;
    using ICommand = System.Windows.Input.ICommand;
    using ProcessApplication = JanHafner.Smartbar.ProcessApplication.ProcessApplication;

    internal sealed class ProcessApplicationButtonViewModel : ApplicationViewModel
    {
        [NotNull]
        private readonly ISmartbarService smartbarService;

        [NotNull]
        private readonly IPluginService pluginService;

        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private ISmartbarSettings smartbarSettings;

        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly IUIExtensionService uiExtensionService;

        [NotNull]
        private readonly WpfHotKeyManager wpfHotKeyManager;

        [CanBeNull]
        private String name;

        [CanBeNull]
        private ImageSource image;

        [NotNull]
        private ApplicationImage applicationImage;

        private Boolean stretchSmallImage;

        [CanBeNull]
        private IImageVisualizationHandler currentImageVisualizationHandler;

        public ProcessApplicationButtonViewModel([NotNull] ProcessApplication processApplication,
            [NotNull] ISmartbarService smartbarService, [NotNull] IPluginService pluginService,
            [NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ISmartbarSettings smartbarSettings,
            [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IUIExtensionService uiExtensionService, [NotNull] WpfHotKeyManager wpfHotKeyManager)
        {
            if (uiExtensionService == null)
            {
                throw new ArgumentNullException(nameof(uiExtensionService));
            }

            if (wpfHotKeyManager == null)
            {
                throw new ArgumentNullException(nameof(wpfHotKeyManager));
            }

            if (processApplication == null)
            {
                throw new ArgumentNullException(nameof(processApplication));
            }

            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (pluginService == null)
            {
                throw new ArgumentNullException(nameof(pluginService));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            this.smartbarService = smartbarService;
            this.pluginService = pluginService;
            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.smartbarSettings = smartbarSettings;
            this.commandDispatcher = commandDispatcher;
            this.uiExtensionService = uiExtensionService;
            this.wpfHotKeyManager = wpfHotKeyManager;

            this.InitializeSubscriptions();

            this.Id = processApplication.Id;
            this.Name = processApplication.Name;
            this.stretchSmallImage = processApplication.StretchSmallImage;

            this.ApplicationImage = processApplication.Image;
           
            this.RefreshImage(this.ApplicationImage.GetType());
        }

        private void InitializeSubscriptions()
        {
            this.SubscriptionTokens.Add(this.eventAggregator.GetEvent<ApplicationImageUpdated>().Subscribe(
                data =>
                {
                    this.ApplicationImage = this.smartbarService.GetApplication<ProcessApplication>(this.Id).Image;

                    this.RefreshImage(data.OldApplicationImageType);
                }, ThreadOption.PublisherThread, true, data => this.Id == data.ApplicationWithImageId));
            this.SubscriptionTokens.Add(this.eventAggregator.GetEvent<ProcessApplicationRenamed>().Subscribe(
              data =>
              {
                  this.Name = data.Name;
              }, ThreadOption.PublisherThread, true, data => this.Id == data.ApplicationId));
            this.SubscriptionTokens.Add(this.eventAggregator.GetEvent<ApplicationUpdated>().Subscribe(
                data =>
                {
                    this.StretchSmallImage = this.smartbarService.GetApplication<ProcessApplication>(this.Id).StretchSmallImage;
                }, ThreadOption.PublisherThread, true, applicationId => this.Id == applicationId));
            this.SubscriptionTokens.Add(this.eventAggregator.GetEvent<CommandHandlerFaulted>().Subscribe(async data =>
            {
                await this.windowService.ShowSimpleModalErrorDialog(String.Format(Dialogs.ApplicationExecutionFailedDialogDescriptionText, this.name), Dialogs.ApplicationExecutionFailedDialogTitle);
            }, ThreadOption.PublisherThread, true, data => data.Command is ExecuteProcessApplicationCommand && ((ExecuteProcessApplicationCommand)data.Command).ApplicationId == this.Id));
        }

        public Guid Id { get; private set; }

        [CanBeNull]
        public String Name
        {
            get { return this.name; }
            set { this.SetProperty(ref this.name, value); }
        }

        public Boolean StretchSmallImage
        {
            get
            {
                return this.stretchSmallImage;
            }
            private set
            {
                this.stretchSmallImage = value;
                this.OnPropertyChanged(() => this.StretchSmallImage);
            }
        }

        [CanBeNull]
        public ImageSource Image
        {
            get
            {
                return this.image;
            }
            private set
            {
                this.SetProperty(ref this.image, value);
                this.SomethingIsWrong = value == null;
            }
        }

        [NotNull]
        public ApplicationImage ApplicationImage
        {
            get
            {
                return this.applicationImage;
            }
            private set
            {
                this.applicationImage = value;
                this.OnPropertyChanged(() => this.ApplicationImage);
            }
        }
        
        [NotNull]
        public ICommand ShowApplicationPreferencesCommand
        {
            get
            {
                return new ApplicationViewModelEditApplicationCommand(this, this.windowService, this.commandDispatcher, this.smartbarService, this.wpfHotKeyManager);
            }
        }

        [NotNull]
        public IEnumerable<IDynamicUICommand> SelectApplicationVisualizationCommands
        {
            get
            {
                return this.uiExtensionService.CreateImageVisualizationCommands(this.Id, () => true);
            }
        }

        [NotNull]
        public ICommand ExecuteCommand
        {
            get
            {
                return new ProcessApplicationButtonViewModelExecuteCommand(this, this.commandDispatcher);
            }
        }

        [NotNull]
        public ICommand DeleteApplicationCommand
        {
            get
            {
                return new ApplicationViewModelDeleteCommand(this, this.smartbarSettings, this.windowService, this.commandDispatcher);
            }
        }

        [NotNull]
        public ICommand MouseEnterApplicationButtonCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.eventAggregator.GetEvent<UpdateStatusbarText>().Publish(this.Name);
                });
            }
        }

        [NotNull]
        public ICommand MouseLeaveApplicationButtonCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    this.eventAggregator.GetEvent<UpdateStatusbarText>().Publish(String.Empty);
                });
            }
        }

        private void RefreshImage(Type oldApplicationImageType)
        {
            if (this.ApplicationImage is IconPackApplicationImage)
            {
                this.SomethingIsWrong = false;
                return;
            }

            if (this.currentImageVisualizationHandler == null || this.ApplicationImage.GetType() != oldApplicationImageType)
            {
                this.currentImageVisualizationHandler = this.pluginService.TryFindHandler(this.ApplicationImage);
                if (this.currentImageVisualizationHandler == null)
                {
                    this.Image = null;

                    return;
                }
            }

            try
            {
                this.Image = this.currentImageVisualizationHandler
                  .Visualize(this.ApplicationImage,
                      new Size(this.smartbarSettings.GridCellContentSize, this.smartbarSettings.GridCellContentSize));
            }
            catch
            {
                this.Image = null;
            }
        }
    }
}
