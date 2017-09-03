namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using JanHafner.Toolkit.Windows;
    using JetBrains.Annotations;

    public sealed class UpdateApplicationWithImageIconApplicationImageCommand : IUpdateApplicationImageCommand
    {
        public UpdateApplicationWithImageIconApplicationImageCommand(Guid applicationWithImageId, [NotNull] String file, Int32 identifier, IconIdentifierType identifierType)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            this.ApplicationWithImageId = applicationWithImageId;
            this.File = file;
            this.Identifier = identifier;
            this.IdentifierType = identifierType;
        }

        public Guid ApplicationWithImageId { get; private set; }

        [NotNull]
        public String File { get; private set; }

        public Int32 Identifier { get; private set; }

        public IconIdentifierType IdentifierType { get; private set; }
    }
}
