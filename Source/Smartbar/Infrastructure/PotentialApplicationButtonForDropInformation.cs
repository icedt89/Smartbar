namespace JanHafner.Smartbar.Infrastructure
{
    using System;
    using JanHafner.Smartbar.Common.UserInterface;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;

    internal sealed class PotentialApplicationButtonForDropInformation
    {
        public PotentialApplicationButtonForDropInformation([NotNull] ApplicationButton applicationButton,
            [NotNull] IApplicationCreationHandler applicationCreationHandler,
            [NotNull] PositionInformation positionInformation, [NotNull] Object data)
        {
            if (applicationButton == null)
            {
                throw new ArgumentNullException(nameof(applicationButton));
            }

            if (applicationCreationHandler == null)
            {
                throw new ArgumentNullException(nameof(applicationCreationHandler));
            }

            if (positionInformation == null)
            {
                throw new ArgumentNullException(nameof(positionInformation));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            this.ApplicationButton = applicationButton;
            this.ApplicationCreationHandler = applicationCreationHandler;
            this.PositionInformation = positionInformation;
            this.Data = data;
        }

        public Object Data { get; private set; }

        public ApplicationButton ApplicationButton { get; private set; }

        public IApplicationCreationHandler ApplicationCreationHandler { get; private set; }

        public PositionInformation PositionInformation { get; private set; }

        public ApplicationViewModel ApplicationViewModel
        {
            get { return (ApplicationViewModel)this.ApplicationButton.DataContext; }
        }
    }
}
