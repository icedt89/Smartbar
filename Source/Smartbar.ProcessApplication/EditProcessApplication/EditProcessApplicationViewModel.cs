namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Common.Validation;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Windows;
    using JanHafner.Toolkit.Windows.HotKey;
    using JetBrains.Annotations;
    using MahApps.Metro.Controls;
    using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

    internal sealed class EditProcessApplicationViewModel : ValidatableBindableBase
    {
        [NotNull]
        private readonly IWindowService windowService;

        [CanBeNull]
        private String execute;

        [CanBeNull]
        private String name;

        [CanBeNull]
        private String username;

        [CanBeNull]
        private String workingDirectory;

        [CanBeNull]
        private String arguments;

        [CanBeNull]
        private String password;

        [CanBeNull]
        private ProcessAffinityMask processAffinityMask;

        private Boolean stretchSmallImage;

        [NotNull]
        private SelectableProcessPriorityClass selectedProcessPriorityClass;

        [NotNull]
        private SelectableProcessWindowStyle selectedProcessWindowStyle;

        [CanBeNull]
        private HotKey hotKey;
        
        private EditProcessApplicationViewModel()
        {
            this.AvailableProcessPriorityClasses = SelectableProcessPriorityClass.CreateSelectableProcessPriorityClasses().ToList();
            this.AvailableProcessWindowStyles = SelectableProcessWindowStyle.CreateSelectableProcessWindowStyles().ToList();

            this.processAffinityMask = ProcessAffinityMask.ForSystem();

            this.selectedProcessPriorityClass = this.AvailableProcessPriorityClasses.First(ppc => ppc.ProcessPriorityClass == ProcessPriorityClass.Normal);
            this.selectedProcessWindowStyle = this.AvailableProcessWindowStyles.First(ppc => ppc.ProcessWindowStyle == ProcessWindowStyle.Normal);

            this.AddCustomPropertyValidation(nameof(this.HotKey), (value, results) =>
            {
                if (value == null)
                {
                    return;
                }

                var hotKeyValue = (HotKey)value;
                var virtualKeyCode = KeyInterop.VirtualKeyFromKey(hotKeyValue.Key);

                if (!GlobalHotKey.Probe((HotKeyModifier) hotKeyValue.ModifierKeys, (UInt32) virtualKeyCode))
                {
                    results.Add(new ValidationResult(EditProcessApplicationValidationMessages.HotKeyInvalidOrExists));
                }
            });
        }

        public EditProcessApplicationViewModel([NotNull] IWindowService windowService)
            : this()
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            this.windowService = windowService;
        }

        public EditProcessApplicationViewModel([NotNull] ProcessApplication processApplication, [NotNull] IWindowService windowService)
            : this(windowService)
        {
            if (processApplication == null)
            {
                throw new ArgumentNullException(nameof(processApplication));
            }

            this.windowService = windowService;

            this.selectedProcessPriorityClass = this.AvailableProcessPriorityClasses.First(ppc => ppc.ProcessPriorityClass == processApplication.Priority);
            this.selectedProcessWindowStyle = this.AvailableProcessWindowStyles.First(ppc => ppc.ProcessWindowStyle == processApplication.WindowStyle);
            this.name = processApplication.Name;
            this.execute = processApplication.Execute;
            this.username = processApplication.Username;
            this.workingDirectory = processApplication.WorkingDirectory;
            this.password = processApplication.GetPassword()?.FromSecureString();
            this.arguments = processApplication.Arguments;
            this.stretchSmallImage = processApplication.StretchSmallImage;
            if (processApplication.HotKeyModifier != HotKeyModifier.None && processApplication.HotKey != Key.None)
            {
                this.hotKey = new HotKey(processApplication.HotKey, (ModifierKeys) processApplication.HotKeyModifier);
            }

            if (processApplication.ProcessAffinityMask.HasValue)
            {
                this.processAffinityMask = ProcessAffinityMask.FromAffinityMask((UInt32)processApplication.ProcessAffinityMask.Value);
            }
        }

        [CanBeNull]
        public String Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.SetProperty(ref this.username, value);
            }
        }

        public Boolean StretchSmallImage
        {
            get { return this.stretchSmallImage; }
            set
            {
                this.SetProperty(ref this.stretchSmallImage, value);
            }
        }
        
        [CanBeNull]
        public HotKey HotKey
        {
            get
            {
                return this.hotKey;
            }
            set
            {
                this.SetProperty(ref this.hotKey, value);
            }
        }

        [NotNull]
        public IEnumerable<SelectableProcessWindowStyle> AvailableProcessWindowStyles { get; private set; }

        [NotNull]
        public SelectableProcessWindowStyle SelectedProcessWindowStyle
        {
            get
            {
                return this.selectedProcessWindowStyle;
            }
            set
            {
                this.SetProperty(ref this.selectedProcessWindowStyle, value);
            }
        }

        [NotNull]
        public IEnumerable<SelectableProcessPriorityClass> AvailableProcessPriorityClasses { get; private set; }

        [NotNull]
        public SelectableProcessPriorityClass SelectedProcessPriorityClass
        {
            get
            {
                return this.selectedProcessPriorityClass;
            }
            set
            {
                this.SetProperty(ref this.selectedProcessPriorityClass, value);
            }
        }

        [Required(ErrorMessageResourceName = "NameRequired", ErrorMessageResourceType = typeof(EditProcessApplicationValidationMessages))]
        [CanBeNull]
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

        [CanBeNull]
        [PathExists(ErrorMessageResourceName = "DirectoryMustExist", ErrorMessageResourceType = typeof(EditProcessApplicationValidationMessages), PathValidationType = PathValidationType.Directory)]
        public String WorkingDirectory
        {
            get
            {
                return this.workingDirectory;
            }
            set
            {
                this.SetProperty(ref this.workingDirectory, value);
            }
        }

        [CanBeNull]
        public String Arguments
        {
            get
            {
                return this.arguments;
            }
            set
            {
                this.SetProperty(ref this.arguments, value);
            }
        }

        [CanBeNull]
        [Required(ErrorMessageResourceName = "ExecuteRequired", ErrorMessageResourceType = typeof(EditProcessApplicationValidationMessages))]
        public String Execute
        {
            get
            {
                return this.execute;
            }
            set
            {
                this.SetProperty(ref this.execute, value);
            }
        }

        [CanBeNull]
        public String Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.SetProperty(ref this.password, value);
            }
        }

        [NotNull]
        public ProcessAffinityMask ProcessAffinityMask
        {
            get
            {
                return this.processAffinityMask;
            }
            set
            {
                this.SetProperty(ref this.processAffinityMask, value);
            }
        }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<EditProcessApplicationViewModel>(this, vm => !vm.HasErrors && vm.IsChanged, this.windowService).ObservesProperty(() => this.IsChanged);
            }
        }

        [NotNull]
        public ICommand SelectFileCommand
        {
            get { return new EditProcessApplicationViewModelSelectFileCommand(this, this.windowService); }
        }

        [NotNull]
        public ICommand SelectDirectoryCommand
        {
            get { return new EditProcessApplicationViewModelSelectDirectoryCommand(this, this.windowService); }
        }

        [NotNull]
        public ICommand SelectWorkingDirectoryCommand
        {
            get { return new EditProcessApplicationViewModelSelectWorkingDirectoryCommand(this, this.windowService); }
        }

        [NotNull]
        public ICommand CancelCommand
        {
            get
            {
                return new CommonCancelCommand<EditProcessApplicationViewModel>(this, this.windowService);
            }
        }

        [NotNull]
        public ICommand DetermineApplicationNameCommand
        {
            get
            {
                return new EditProcessApplicationViewModelDetermineApplicationNameCommand(this).ObservesProperty(() => this.Execute);
            }
        }

        [NotNull]
        public ICommand SetProcessAffinityMaskCommand
        {
            get { return new EditProcessApplicationViewModelSetProcessAffinityMaskCommand(this, this.windowService); }
        }
    }
}