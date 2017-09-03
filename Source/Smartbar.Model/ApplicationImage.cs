namespace JanHafner.Smartbar.Model
{
    using JetBrains.Annotations;

    public abstract class ApplicationImage
    {
        public abstract void Update([NotNull] ApplicationImage applicationImage);
    }
}
