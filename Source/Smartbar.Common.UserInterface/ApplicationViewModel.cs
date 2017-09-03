namespace JanHafner.Smartbar.Common.UserInterface
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Prism.Events;
    using Prism.Mvvm;

    public abstract class ApplicationViewModel : BindableBase, IDisposable
    {
        private Boolean somethingIsWrong;

        [NotNull]
        protected readonly ICollection<SubscriptionToken> SubscriptionTokens;

        protected ApplicationViewModel()
        {
            this.SubscriptionTokens = new List<SubscriptionToken>();
        }

        public Int32 Row { get; internal set; }

        public Int32 Column { get; internal set; }

        public Guid GroupId { get; internal set; }

        public Boolean SomethingIsWrong
        {
            get { return this.somethingIsWrong; }
            protected set { this.SetProperty(ref this.somethingIsWrong, value); }
        }

        public void Dispose()
        {
            this.SubscriptionTokens.UnsubscribeAll();
        }
    }
}
