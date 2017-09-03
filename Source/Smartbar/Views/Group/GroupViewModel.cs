namespace JanHafner.Smartbar.Views.Group
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Infrastructure;
    using JanHafner.Smartbar.Infrastructure.Commanding.Groups;
    using JanHafner.Smartbar.Model;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Events;
    using Prism.Mvvm;
    using Application = JanHafner.Smartbar.Model.Application;
    using ICommand = System.Windows.Input.ICommand;

    internal sealed class GroupViewModel : BindableBase
    {
        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly ICommandDispatcher commandDispatcher;

        [NotNull]
        private readonly ISmartbarService smartbarService;

        [NotNull]
        private readonly ICollection<SubscriptionToken> subscriptionTokens;

        [NotNull]
        private readonly ISmartbarSettings smartbarSettings;

        [NotNull]
        private readonly IApplicationButtonFactory applicationButtonFactory;

        [NotNull]
        private readonly IPluginService pluginService;

        [NotNull]
        private String name;

        private Boolean canShiftRight;

        private Boolean canBeDeleted;

        public GroupViewModel([NotNull] Group group, [NotNull] IEventAggregator eventAggregator, [NotNull] IWindowService windowService,
            [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] ISmartbarService smartbarService,
            [NotNull] ISmartbarSettings smartbarSettings, [NotNull] IApplicationButtonFactory applicationButtonFactory,
            [NotNull] IPluginService pluginService)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(@group));
            }
            
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (commandDispatcher == null)
            {
                throw new ArgumentNullException(nameof(commandDispatcher));
            }

            if (smartbarService == null)
            {
                throw new ArgumentNullException(nameof(smartbarService));
            }

            if (smartbarSettings == null)
            {
                throw new ArgumentNullException(nameof(smartbarSettings));
            }

            if (applicationButtonFactory == null)
            {
                throw new ArgumentNullException(nameof(applicationButtonFactory));
            }

            if (pluginService == null)
            {
                throw new ArgumentNullException(nameof(pluginService));
            }

            this.smartbarSettings = smartbarSettings;
            this.applicationButtonFactory = applicationButtonFactory;
            this.pluginService = pluginService;
            this.subscriptionTokens = new List<SubscriptionToken>();
            this.eventAggregator = eventAggregator;
            this.windowService = windowService;
            this.commandDispatcher = commandDispatcher;
            this.smartbarService = smartbarService;

            this.Id = group.Id;
            this.name = group.Name;
            this.IsSelected = group.IsSelected;
            this.Applications = new ObservableCollection<Application>(group.Applications);
            this.Position = group.Position;

            this.InitializeSubscriptions();
        }

        private void InitializeSubscriptions()
        {
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupUnselected>().Subscribe(_ =>
            {
                this.IsSelected = false;
            }, ThreadOption.PublisherThread, true, group => group.Id == this.Id));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupSelected>().Subscribe(_ =>
            {
                this.IsSelected = true;
            }, ThreadOption.PublisherThread, true, group => group.Id == this.Id));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupDeleted>().Subscribe(_ =>
            {
                this.subscriptionTokens.UnsubscribeAll();
            }, ThreadOption.PublisherThread, true, data => data.Id == this.Id));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupCleared>().Subscribe(_ =>
            {
                this.Applications.Clear();
            }, ThreadOption.PublisherThread, true, data => data.Id == this.Id));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<GroupRenamed>().Subscribe(data =>
            {
                this.Name = data.Group.Name;
            }, ThreadOption.PublisherThread, true, data => data.Group.Id == this.Id));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<ApplicationsDeleted>().Subscribe(data =>
            {
                foreach (var application in data.Applications)
                {
                    this.Applications.Remove(application);
                }
            }, ThreadOption.PublisherThread, true, data => this.Id == data.Group.Id));
            this.subscriptionTokens.Add(this.eventAggregator.GetEvent<ApplicationsCreated>().Subscribe(data =>
            {
                foreach (var application in data.Applications)
                {
                    this.Applications.Add(application);
                }
            }, ThreadOption.PublisherThread, true, data => this.Id == data.Group.Id));
        }

        public ICommand DragEnterCommand
        {
            get
            {
                return new DelegateCommand<DragEventArgs>(async a =>
                {

                    if (!this.IsSelected)
                    {
                        await this.commandDispatcher.DispatchAsync(new SelectGroupCommand(this.Id));
                    }
                });
            }
        }

        public Guid Id { get; private set; }

        [NotNull]
        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.SetProperty(ref this.name, value);
            }
        }

        #region Drop Behavior

        public async Task ImportApplicationsAsync(IEnumerable<ImportApplicationInformation> importApplicationInformations)
        {
            var importApplicationInformationsList = importApplicationInformations.ToList();

            foreach (var importedApplicationInformation in importApplicationInformationsList)
            {
                this.ProcessImportApplicationInformation(importedApplicationInformation);
            }

            if (!(await this.ProceedOnFailedDropsAsync(importApplicationInformationsList)))
            {
                return;
            }

            foreach (var createApplicationContainerCommand in importApplicationInformationsList)
            {
                createApplicationContainerCommand.CreatedCommand.TargetColumn = createApplicationContainerCommand.Destination.Column;
                createApplicationContainerCommand.CreatedCommand.TargetRow = createApplicationContainerCommand.Destination.Row;
            }

            var createApplicationsCommand = new CreateApplicationsCommand();
            foreach (var processableCommand in importApplicationInformationsList.Where(sdr => sdr.Processable))
            {
                createApplicationsCommand.Add(processableCommand.CreatedCommand);
            }

            await this.commandDispatcher.DispatchAsync(new[] { createApplicationsCommand });
        }

        private void ProcessImportApplicationInformation(ImportApplicationInformation importApplicationInformation)
        {
            ICreateApplicationCommand createApplicationCommand = null;
            try
            {
                createApplicationCommand = importApplicationInformation.ApplicationCreationHandler.CreateCommand(importApplicationInformation.Data);
                createApplicationCommand.ApplicationCreateTargetBehavior = importApplicationInformation.ApplicationCreateTargetBehavior;
            }
            catch (Exception exception)
            {
                importApplicationInformation.OccuredException = exception;
                return;
            }

            createApplicationCommand.TargetGroupId = this.Id;

            importApplicationInformation.CreatedCommand = createApplicationCommand;
        }

        private async Task<Boolean> ProceedOnFailedDropsAsync(IReadOnlyCollection<ImportApplicationInformation> importApplicationInformations)
        {
            var dropsWithError = importApplicationInformations.Where(sdr => sdr.OccuredException != null).ToList();
            if (dropsWithError.Any())
            {
                this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(new AggregateException(dropsWithError.Select(dwe => dwe.OccuredException))));

                if (dropsWithError.Count == importApplicationInformations.Count)
                {
                    await this.windowService.ShowSimpleModalDialog(
                        String.Format(Extensibility.Localization.UserMessages.ErrorsOccuredDuringDropOperationMessage,
                            String.Join(Environment.NewLine, dropsWithError.Select(sdr => sdr.Data))), Extensibility.Localization.UserMessages.ErrorsOccuredDuringDropOperationTitle,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                if (await this.windowService.ShowSimpleModalDialog(
                    String.Format(Extensibility.Localization.UserMessages.ErrorsOccuredDuringDropOperationCreateTheLeftMessage,
                        String.Join(Environment.NewLine, dropsWithError.Select(sdr => sdr.Data))), Extensibility.Localization.UserMessages.ErrorsOccuredDuringDropOperationTitle,
                    MessageBoxButton.YesNo, MessageBoxImage.Exclamation) ==
                    MessageBoxResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        [NotNull]
        [LinqTunnel]
        public IEnumerable<Tuple<Object, IApplicationCreationHandler>> FindApplicationCreationHandlers(IEnumerable<Object> translatedData)
        {
            return translatedData.Select(data => new Tuple<Object, IApplicationCreationHandler>(data, this.pluginService.TryFindHandler(data)));
        } 

        [NotNull]
        [LinqTunnel]
        public IEnumerable<IDataObjectTranslator> FindDataObjectTranslators(IDataObject dataObject)
        {
            return this.pluginService.FindDataObjectTranslators(dataObject);
        }

        [NotNull]
        [LinqTunnel]
        public IEnumerable<PositionInformation> GetPositionInformation(Int32 startRowIndex, Int32 startColumnIndex)
        {
            return this.smartbarService.GetPositionInformation(this.Id, startColumnIndex, startRowIndex);
        }

        [NotNull]
        public ApplicationButton CreateApplicationButton(Application application, Int32 column, Int32 row)
        {
            return this.applicationButtonFactory.CreateApplicationButton(application, this.Id, column, row);
        }

        public Boolean IsSelected { get; private set; }

        public Int32 Position { get; private set; }

        public Boolean CanShiftLeft
        {
            get
            {
                return this.Position > 0;
            }
        }

        public Boolean CanBeDeleted
        {
            get
            {
                return this.canBeDeleted;
            }
            set { this.SetProperty(ref this.canBeDeleted, value); }
        }

        public Boolean CanShiftRight
        {
            get
            {
                return this.canShiftRight;
            }
            set { this.SetProperty(ref this.canShiftRight, value); }
        }

        public void Reposition(Int32 position)
        {
            this.Position = position;

            this.OnPropertyChanged(() => this.CanShiftLeft);
        }

        [NotNull]
        public ICommand DeleteWithMiddleMouseButtonCommand
        {
            get
            {
                return new GroupViewModelDeleteWithMiddleMouseButtonCommand(this, this.smartbarSettings, this.windowService, this.commandDispatcher)
                    .ObservesProperty(() => this.CanBeDeleted);
            }
        }

        [NotNull]
        public ICommand ClearCommand
        {
            get
            {
                return new GroupViewModelClearCommand(this, this.smartbarSettings, this.windowService, this.commandDispatcher);
            }
        }

        [NotNull]
        public ICommand DirectlyEditedGroupNameAcceptedCommand
        {
            get
            {
                return new DelegateCommand<String>(async newValue =>
                {
                    await this.commandDispatcher.DispatchAsync(new RenameGroupCommand(this.Id, newValue));
                });
            }
        }

        [NotNull]
        public ICommand DeleteCommand
        {
            get
            {
                return new GroupViewModelDeleteCommand(this, this.smartbarSettings, this.windowService, this.commandDispatcher).ObservesProperty(() => this.CanBeDeleted);
            }
        }

        [NotNull]
        public ICommand ShiftLeftCommand
        {
            get
            {
                return new GroupViewModelShiftLeftCommand(this, this.commandDispatcher).ObservesProperty(() => this.CanShiftLeft);
            }
        }

        [NotNull]
        public ICommand ShiftRightCommand
        {
            get
            {
                return new GroupViewModelShiftRightCommand(this, this.commandDispatcher).ObservesProperty(() => this.CanShiftRight);
            }
        }

        [NotNull]
        public ICommand RenameCommand
        {
            get
            {
                return new GroupViewModelRenameCommand(this, this.windowService, this.commandDispatcher);
            }
        }

        [NotNull]
        public ObservableCollection<Application> Applications { get; private set; }
    }
}
