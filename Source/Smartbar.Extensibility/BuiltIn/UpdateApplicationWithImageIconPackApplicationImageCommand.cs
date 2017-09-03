namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using JetBrains.Annotations;

    public sealed class UpdateApplicationWithImageIconPackApplicationImageCommand : IUpdateApplicationImageCommand
    {
        public UpdateApplicationWithImageIconPackApplicationImageCommand(Guid applicationWithImageId,
            [NotNull] Type iconPackType,
            [NotNull] Int32 iconPackKindKey, [NotNull] Byte[] fillColor)
        {
            if (iconPackType == null)
            {
                throw new ArgumentNullException(nameof(iconPackType));
            }
            
            if (fillColor == null || fillColor.Length == 0)
            {
                throw new ArgumentNullException(nameof(fillColor));
            }

            this.ApplicationWithImageId = applicationWithImageId;
            this.FillColor = fillColor;
            this.IconPackKindKey = iconPackKindKey;
            this.IconPackType = iconPackType;
        }

        public Guid ApplicationWithImageId { get; private set; }

        [NotNull]
        public Type IconPackType { get; private set; }

        [NotNull]
        public Byte[] FillColor { get; private set; }

        [NotNull]
        public Int32 IconPackKindKey { get; private set; }
    }
}
