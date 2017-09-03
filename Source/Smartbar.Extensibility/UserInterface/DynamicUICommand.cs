namespace JanHafner.Smartbar.Extensibility.UserInterface
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Prism.Commands;

    public abstract class DynamicUICommand : DelegateCommand, IDynamicUICommand
    {
        [NotNull]
        private readonly Func<String> displayTextFactory;

        [NotNull]
        private readonly ICollection<IDynamicUICommand> childMenuItems;

        protected DynamicUICommand(Func<String> displayTextFactory, Action executeMethod, Func<Boolean> canExecuteMethod)
            : base(executeMethod, canExecuteMethod)
        {
            this.displayTextFactory = displayTextFactory;
            if (displayTextFactory == null)
            {
                throw new ArgumentNullException(nameof(displayTextFactory));
            }

            this.childMenuItems = new List<IDynamicUICommand>();
        }

        public virtual String DisplayText
        {
            get { return this.displayTextFactory(); }
        }

        public IEnumerable<IDynamicUICommand> ChildMenuItems
        {
            get
            {
                return this.childMenuItems;
            }
        }

        public void AddChildMenuItem([NotNull] IDynamicUICommand dynamicUiCommand)
        {
            if (dynamicUiCommand == null)
            {
                throw new ArgumentNullException(nameof(dynamicUiCommand));
            }

            this.childMenuItems.Add(dynamicUiCommand);
        }
    }
}
