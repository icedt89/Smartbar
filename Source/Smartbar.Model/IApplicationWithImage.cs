namespace JanHafner.Smartbar.Model
{
    using System;
    using JetBrains.Annotations;

    public interface IApplicationWithImage
    {
        Guid Id { get; }

        ApplicationImage Image { get; }

        void UpdateImage([NotNull] ApplicationImage applicationImage);

        void DeleteImage();
    }
}