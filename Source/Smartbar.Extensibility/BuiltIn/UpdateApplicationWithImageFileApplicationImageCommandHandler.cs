namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using System.ComponentModel.Composition;
    using System.Linq;
    using System.Threading.Tasks;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.Commanding.Events;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using Prism.Events;

    [Export(typeof(ICommandHandler))]
    public sealed class UpdateApplicationWithImageFileApplicationImageCommandHandler : CommandHandler<UpdateApplicationWithImageFileApplicationImageCommand>
    {
        [NotNull]
        private readonly ISmartbarDbContext smartbarDbContext;

        [ImportingConstructor]
        public UpdateApplicationWithImageFileApplicationImageCommandHandler([NotNull] IEventAggregator eventAggregator,
            [NotNull]  ISmartbarDbContext smartbarDbContext)
            : base(eventAggregator)
        {
            if (smartbarDbContext == null)
            {
                throw new ArgumentNullException(nameof(smartbarDbContext));
            }

            this.smartbarDbContext = smartbarDbContext;
        }

        public override async Task HandleAsync(UpdateApplicationWithImageFileApplicationImageCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            var updatedApplication = this.smartbarDbContext.Groups.SelectMany(g => g.Applications).OfType<IApplicationWithImage>()
                    .Single(application => application.Id == command.ApplicationWithImageId);

            var oldApplicationImageType = updatedApplication.Image.GetType();

            updatedApplication.UpdateImage(new FileApplicationImage(command.File));

            await this.smartbarDbContext.SaveChangesAsync();

            this.EventAggregator.GetEvent<ApplicationImageUpdated>().Publish(new ApplicationImageUpdated.Data(command.ApplicationWithImageId, oldApplicationImageType, typeof(FileApplicationImage)));
            this.PublishCommandHandlerDone(command);
        }
    }
}
