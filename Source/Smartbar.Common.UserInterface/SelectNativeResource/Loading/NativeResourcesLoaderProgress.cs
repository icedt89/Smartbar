namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.Loading
{
    using System;
    using JetBrains.Annotations;

    internal sealed class NativeResourcesLoaderProgress
    {
        public NativeResourcesLoaderProgress([NotNull] IconImageSourceBag iconImageSourceBag, Boolean isFinished)
        {
            if (iconImageSourceBag == null)
            {
                throw new ArgumentNullException(nameof(iconImageSourceBag));
            }

            this.IconImageSourceBag = iconImageSourceBag;
            this.IsFinished = isFinished;
        }

        [NotNull]
        public IconImageSourceBag IconImageSourceBag { get; private set; }

        public Boolean IsFinished { get; private set; }
    }
}