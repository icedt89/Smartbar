namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.Loading;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown;
    using JanHafner.Smartbar.Common.Validation;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;
    using Prism.Events;

    public sealed class SelectNativeResourceViewModel : ValidatableBindableBase, IDisposable
    {
        [NotNull]
        private readonly IWindowService windowService;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [NotNull]
        private readonly IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider;

        [CanBeNull]
        private String file;

        [CanBeNull]
        private IconImageSourceBag icon;

        [CanBeNull]
        private Int32? preselectIdentifier;

        [CanBeNull]
        private IconIdentifierType? preselectIconIdentifierType;

        [NotNull]
        private readonly ObservableCollection<IconImageSourceBag> icons;

        private Boolean iconsCouldNotBeExtractedFromFile;

        private Boolean noIconsPresentInFile;

        private Boolean isRefreshingImages;

        private Boolean extractionAborted;

        private Int32 maximalVisualizableImages;

        private Int32 currentVisualizationProgress;

        [CanBeNull]
        private CancellationTokenSource cancellationTokenSource;

        [NotNull]
        private readonly NativeResourcesLoader nativeResourcesLoader;

        public SelectNativeResourceViewModel([NotNull] IWindowService windowService,
            [NotNull] IEventAggregator eventAggregator,
            [NotNull] IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            if (wellKnownIconLibraryUiCommandProvider == null)
            {
                throw new ArgumentNullException(nameof(wellKnownIconLibraryUiCommandProvider));
            }

            this.windowService = windowService;
            this.eventAggregator = eventAggregator;
            this.wellKnownIconLibraryUiCommandProvider = wellKnownIconLibraryUiCommandProvider;
            this.icons = new ObservableCollection<IconImageSourceBag>();
            this.nativeResourcesLoader = new NativeResourcesLoader(new RunOnDispatcherProgress<NativeResourcesLoaderProgress>(progress =>
            {
                this.CurrentVisualizationProgress++;

                if(this.isRefreshingImages)
                {
                    this.icons.Add(progress.IconImageSourceBag);

                    if (this.icon == null && this.preselectIdentifier.HasValue && this.preselectIconIdentifierType.HasValue)
                    {
                        this.Icon = this.icons.FirstOrDefault(icon => icon.IconIdentifierType == this.preselectIconIdentifierType.Value && icon.Identifier == this.preselectIdentifier.Value);
                    }
                }

                if (progress.IsFinished || (this.cancellationTokenSource != null && this.cancellationTokenSource.IsCancellationRequested))
                {
                    this.IsRefreshingImages = false;
                }
            }));
        }

        [CanBeNull]
        [Required(ErrorMessageResourceType = typeof(SelectNativeResourceValidationMessages), ErrorMessageResourceName = "FileMustExist")]
        [PathExists(ErrorMessageResourceType = typeof(SelectNativeResourceValidationMessages), ErrorMessageResourceName = "FileMustExist", PathValidationType = PathValidationType.File)]
        public String File
        {
            get
            {
                return this.file;
            }
            set { this.SetProperty(ref this.file, value); }
        }

        [Required]
        [CanBeNull]
        public IconImageSourceBag Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.SetProperty(ref this.icon, value);
            }
        }

        [NotNull]
        public IEnumerable<ICommand> WellKnownIconLibraries
        {
            get
            {
                return this.wellKnownIconLibraryUiCommandProvider.GetWellKnownIconLibraries(this);
            }
        }

        [NotNull]
        public IEnumerable<IconImageSourceBag> Icons
        {
            get
            {
                return this.icons;
            }
        }

        private async Task LoadImagesAsync()
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            
            this.IconsCouldNotBeExtractedFromFile = false;
            this.NoIconsPresentInFile = false;
            this.ExtractionAborted = false;
            this.MaximalVisualizableImages = 0;
            this.CurrentVisualizationProgress = 0;
            this.IsRefreshingImages = true;

            this.icons.Clear();
            this.Icon = null;

            try
            {
                this.MaximalVisualizableImages = await this.nativeResourcesLoader.LoadAsync(this.file, this.cancellationTokenSource.Token);
                this.NoIconsPresentInFile = this.MaximalVisualizableImages == 0;
                if (this.noIconsPresentInFile)
                {
                    this.IsRefreshingImages = false;
                }
            }
            catch (Exception ex)
            {
                if (!(ex is OperationCanceledException))
                {
                    this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));
                }

                this.IconsCouldNotBeExtractedFromFile = true;
                this.IsRefreshingImages = false;
            }
        }

        public Task LoadImagesAsync([NotNull] String file)
        {
            if (!System.IO.File.Exists(file))
            {
                throw new FileNotFoundException(null, file);
            }

            this.File = file;

            return this.LoadImagesAsync();
        }

        public Task LoadImagesAsync([NotNull] String file, Int32 preselectedIconIdentifier, IconIdentifierType preselectIconIdentifierType)
        {
            if (!System.IO.File.Exists(file))
            {
                throw new FileNotFoundException(null, file);
            }

            this.File = file;
            this.preselectIdentifier = preselectedIconIdentifier;
            this.preselectIconIdentifierType = preselectIconIdentifierType;

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

        public Boolean IsRefreshingImages
        {
            get
            {
                return this.isRefreshingImages;
            }
            private set
            {
                if (this.SetProperty(ref this.isRefreshingImages, value) && !value)
                {
                    this.UnsetCancellationToken();
                    this.preselectIdentifier = null;
                    this.preselectIconIdentifierType = null;
                }
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

        public Boolean IconsCouldNotBeExtractedFromFile
        {
            get
            {
                return this.iconsCouldNotBeExtractedFromFile;
            }
            private set
            {
                this.SetProperty(ref this.iconsCouldNotBeExtractedFromFile, value);
            }
        }

        public Boolean NoIconsPresentInFile
        {
            get
            {
                return this.noIconsPresentInFile;
            }
            private set
            {
                this.SetProperty(ref this.noIconsPresentInFile, value);
            }
        }

        public Boolean ExtractionAborted
        {
            get { return this.extractionAborted; }
            private set { this.SetProperty(ref this.extractionAborted, value); }
        }

        [NotNull]
        public ICommand CancelCommand
        {
            get
            {
                return new CommonCancelCommand<SelectNativeResourceViewModel>(this, viewModel =>
                {
                    if (viewModel.IsRefreshingImages)
                    {
                        viewModel.IsRefreshingImages = false;
                        viewModel.ExtractionAborted = true;

                        throw new AbortCloseException();
                    }
                }, this.windowService);
            }
        }

        [NotNull]
        public ICommand SelectFileCommand
        {
            get
            {
                return new SelectNativeResourceViewModelSelectFileCommand(this, this.windowService).ObservesProperty(() => this.IsRefreshingImages);
            }
        }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<SelectNativeResourceViewModel>(this, viewModel => !viewModel.HasErrors && viewModel.IsChanged && !viewModel.IsRefreshingImages, this.windowService)
                    .ObservesProperty(() => this.IsChanged);
            }
        }

        public void Dispose()
        {
            this.cancellationTokenSource?.Cancel();
        }
    }
}
