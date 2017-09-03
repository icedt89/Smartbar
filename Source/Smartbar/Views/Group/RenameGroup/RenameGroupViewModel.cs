namespace JanHafner.Smartbar.Views.Group.RenameGroup
{
    using System;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Common.Validation;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class RenameGroupViewModel : ValidatableBindableBase
    {
        [NotNull]
        private readonly IWindowService windowService;

        [CanBeNull]
        private String newGroupName;

        public RenameGroupViewModel([NotNull] String groupName, [NotNull] IWindowService windowService)
        {
            if (String.IsNullOrWhiteSpace(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            this.windowService = windowService;

            this.CurrentGroupName = groupName;
            this.newGroupName = this.CurrentGroupName;
        }

        [Required(ErrorMessageResourceType = typeof(GroupValidationMessages), ErrorMessageResourceName = "GroupNameRequired")]
        [CanBeNull]
        public String NewGroupName
        {
            get { return this.newGroupName; }
            set
            {
                this.SetProperty(ref this.newGroupName, value);
            }
        }

        [NotNull]
        public String CurrentGroupName { get; private set; }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<RenameGroupViewModel>(this, viewModel => !viewModel.HasErrors && viewModel.IsChanged, this.windowService).ObservesProperty(() => this.IsChanged);
            }
        }
    }
}