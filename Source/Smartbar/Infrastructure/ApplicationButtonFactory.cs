namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Linq;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Controls.FaultedApplicationButton;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(IApplicationButtonFactory))]
    internal sealed class ApplicationButtonFactory : IApplicationButtonFactory
    {
        [NotNull] 
        private readonly IEnumerable<IApplicationButtonProvider> applicationButtonProvider;

        [NotNull]
        private readonly IFaultedApplicationButtonProvider faultedApplicationButtonProvider;

        [NotNull]
        private readonly IEventAggregator eventAggregator;

        [ImportingConstructor]
        public ApplicationButtonFactory([NotNull, ImportMany(typeof (IApplicationButtonProvider))] IEnumerable<IApplicationButtonProvider> applicationButtonProvider,
            [NotNull] IFaultedApplicationButtonProvider faultedApplicationButtonProvider,
            [NotNull] IEventAggregator eventAggregator)
        {
            if (applicationButtonProvider == null)
            {
                throw new ArgumentNullException(nameof(applicationButtonProvider));
            }

            if (faultedApplicationButtonProvider == null)
            {
                throw new ArgumentNullException(nameof(faultedApplicationButtonProvider));
            }

            if (eventAggregator == null)
            {
                throw new ArgumentNullException(nameof(eventAggregator));
            }

            this.applicationButtonProvider = applicationButtonProvider;
            this.faultedApplicationButtonProvider = faultedApplicationButtonProvider;
            this.eventAggregator = eventAggregator;
        }

        public ApplicationButton CreateApplicationButton(Application application, Guid groupId, Int32 column, Int32 row)
        {
            ApplicationButton applicationButton;
            try
            {
                var applicationButtonFactory = this.applicationButtonProvider.Single(abf => abf.CanCreateApplicationButton(application));

                applicationButton = applicationButtonFactory.CreateApplicationButton(application);
            }
            catch (Exception ex)
            {
                this.eventAggregator.GetEvent<ExceptionNotification>().Publish(new ExceptionNotification.Data(ex));

                applicationButton = this.faultedApplicationButtonProvider.CreateApplicationButton(application);
            }

            var applicationButtonViewModel = (ApplicationViewModel) applicationButton.DataContext;

            applicationButtonViewModel.Row = row;
            applicationButtonViewModel.Column = column;
            applicationButtonViewModel.GroupId = groupId;

            return applicationButton;
        }
    }
}
