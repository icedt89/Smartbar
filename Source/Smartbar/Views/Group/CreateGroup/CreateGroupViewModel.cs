namespace JanHafner.Smartbar.Views.Group.CreateGroup
{
    using System;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Common.Validation;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class CreateGroupViewModel : ValidatableBindableBase
    {
        [NotNull]
        private readonly IWindowService windowService;

        [CanBeNull]
        private String groupName;

        public CreateGroupViewModel([NotNull] IWindowService windowService)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            this.windowService = windowService;
        }

        [Required(ErrorMessageResourceType = typeof(GroupValidationMessages), ErrorMessageResourceName = "GroupNameRequired")]
        [NotNull]
        public String GroupName
        {
            get { return this.groupName; }
            set
            {
                this.SetProperty(ref this.groupName, value);
            }
        }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<CreateGroupViewModel>(this, viewModel => !viewModel.HasErrors && viewModel.IsChanged, this.windowService).ObservesProperty(() => this.IsChanged);
            }
        }
    }
}