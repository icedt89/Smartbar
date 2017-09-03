namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using JetBrains.Annotations;

    public sealed class UpdateApplicationWithImageFileApplicationImageCommand : IUpdateApplicationImageCommand
    {
        public UpdateApplicationWithImageFileApplicationImageCommand(Guid applicationWithImageId, String file)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            this.ApplicationWithImageId = applicationWithImageId;
            this.File = file;
        }

        public Guid ApplicationWithImageId { get; private set; }

        [NotNull]
        public String File { get; private set; }
    }
}
