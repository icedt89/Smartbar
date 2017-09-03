namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.Loading;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks;
    using JanHafner.Smartbar.Common.Validation;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Commands;
    using Prism.Events;
    using Xceed.Wpf.Toolkit;

    public sealed class SelectIconPackResourceViewModel : ValidatableBindableBase, IDisposable
    {
        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly ISelectableIconPacksProvider selectableIconPacksProvider;

        private Color fillColor;

        private Boolean loadAborted;

        [NotNull]
        private ISelectableIconPack iconPack;

        [CanBeNull]
        private IconPackResourceBag resource;

        private Int32 maximalVisualizableImages;

        private Int32 currentVisualizationProgress;

        [NotNull]
        private readonly ObservableCollection<IconPackResourceBag> resources;

        private Boolean isRefreshingImage;

        [CanBeNull]
        private CancellationTokenSource cancellationTokenSource;

        [NotNull]
        private readonly SelectIconPackResourcesLoader selectIconPackResourcesLoader;

        private Int32? preselectIconPackKindKey;

        public SelectIconPackResourceViewModel([NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator, [NotNull] ISelectableIconPacksProvider selectableIconPacksProvider)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.selectableIconPacksProvider = selectableIconPacksProvider;
            this.fillColor = Colors.Black;
            this.resources = new ObservableCollection<IconPackResourceBag>();
            this.iconPack = this.SelectableIconPacks.First(ip => ip is SelectableIconPackEntypo);
            this.selectIconPackResourcesLoader =
                new SelectIconPackResourcesLoader(new RunOnDispatcherProgress<SelectIconPackResourcesLoaderProgress>(
                    progress =>
                    {
                        this.CurrentVisualizationProgress++;

                        if (this.isRefreshingImage)
                        {
                            this.resources.Add(progress.IconPackResourceBag);

                            if (this.resource == null && this.preselectIconPackKindKey.HasValue)
                            {
                                this.Resource = this.resources.FirstOrDefault(icon => icon.IconPackKindKey == this.preselectIconPackKindKey.Value);
                            }
                        }

                        if (progress.IsFinished || (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested))
                        {
                            this.IsRefreshingImages = false;
                        }
                    }));
        }

        public async Task LoadImagesAsync()
        {
            this.LoadAborted = false;
            this.cancellationTokenSource = new CancellationTokenSource();
            this.CurrentVisualizationProgress = 0;
            this.MaximalVisualizableImages = 0;
            this.IsRefreshingImages = true;

            this.resources.Clear();
            
            try
            {
                this.MaximalVisualizableImages = await this.selectIconPackResourcesLoader.Load(this.iconPack.IconPackType, this.iconPack.IconPackKindType, this.cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                if (!(ex is OperationCanceledException))
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));
                }

                this.IsRefreshingImages = false;
            }
        }

        public Task LoadImagesAsync(Type iconPackType, Int32 preselectIconPackKindKey)
        {
            this.IconPack = this.SelectableIconPacks.First(sip => sip.IconPackType == iconPackType);
            this.preselectIconPackKindKey = preselectIconPackKindKey;

            return this.LoadImagesAsync();
        }

        private void UnsetCancellationToken()
        {
            if (this.cancellationTokenSource == null)
            {
                return;
            }

            if (!this.cancellationTokenSource.IsCancellationRequested)
            {
                this.cancellationTokenSource.Cancel();
            }

            this.cancellationTokenSource = null;
        }

        [NotNull]
        public IEnumerable<IconPackResourceBag> Resources
        {
            get
            {
                return this.resources;
            }
        }

        [NotNull]
        public ISelectableIconPack IconPack
        {
            get
            {
                return this.iconPack;
            }
            set
            {
                if (this.SetProperty(ref this.iconPack, value))
                {
                    // Needed because properties cant be async.
                    Task.Factory.StartNew(async () => await this.LoadImagesAsync(), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());
                }
            }
        }

        [NotNull]
        public IEnumerable<ISelectableIconPack> SelectableIconPacks
        {
            get
            {
                return this.selectableIconPacksProvider.GetSelectableIconPacks();
            }
        }

        [CanBeNull]
        [Required]
        public IconPackResourceBag Resource
        {
            get
            {
                return this.resource;
            }
            set
            {
                this.SetProperty(ref this.resource, value);
            }
        }

        [NotNull]
        public ICommand SelectColorDialogClosed
        {
            get
            {
                return new DelegateCommand<RoutedEventArgs>(routedEventArgs =>
                {
                    var colorPickerDialog = (ColorPicker)routedEventArgs.Source;
                    this.FillColor = colorPickerDialog.SelectedColor ?? Colors.Black;
                });
            }
        }

        public Int32 CurrentVisualizationProgress
        {
            get
            {
                return this.currentVisualizationProgress;
            }
            private set
            {
                this.SetProperty(ref this.currentVisualizationProgress, value);
            }
        }

        public Int32 MaximalVisualizableImages
        {
            get
            {
                return this.maximalVisualizableImages;
            }
            private set
            {
                this.SetProperty(ref this.maximalVisualizableImages, value);
            }
        }

        public Boolean LoadAborted
        {
            get
            {
                return this.loadAborted;
            }
            private set
            {
                this.SetProperty(ref this.loadAborted, value);
            }
        }

        public Color FillColor
        {
            get
            {
                return this.fillColor;
            }
            set
            {
                this.SetProperty(ref this.fillColor, value);
            }
        }

        [NotNull]
        public ICommand CancelCommand
        {
            get { return new CommonCancelCommand<SelectIconPackResourceViewModel>(this, viewModel =>
            {
                if (viewModel.IsRefreshingImages)
                {
                    viewModel.IsRefreshingImages = false;
                    viewModel.LoadAborted = true;

                    throw new AbortCloseException();
                }
            }, this.windowService); }
        }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<SelectIconPackResourceViewModel>(this, viewModel => !viewModel.HasErrors && viewModel.IsChanged && !viewModel.IsRefreshingImages, this.windowService)
                    .ObservesProperty(() => this.IsChanged);
            }
        }

        public Boolean IsRefreshingImages
        {
            get { return this.isRefreshingImage; }
            private set
            {
                if (this.SetProperty(ref this.isRefreshingImage, value) && !value)
                {
                    this.UnsetCancellationToken();
                    this.preselectIconPackKindKey = null;
                }
            }
        }

        public void Dispose()
        {
            this.cancellationTokenSource?.Cancel();
        }
    }
}