namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.Loading
{
    using System;
    using JetBrains.Annotations;

    internal sealed class SelectIconPackResourcesLoaderProgress
    {
        public SelectIconPackResourcesLoaderProgress(
            [NotNull] IconPackResourceBag iconPackResourceBag, Boolean isFinished)
        {
            if (iconPackResourceBag == null)
            {
                throw new ArgumentNullException(nameof(iconPackResourceBag));
            }

            this.IconPackResourceBag = iconPackResourceBag;
            this.IsFinished = isFinished;
        }

        public IconPackResourceBag IconPackResourceBag { get; private set; }

        public Boolean IsFinished { get; private set; }
    }
}