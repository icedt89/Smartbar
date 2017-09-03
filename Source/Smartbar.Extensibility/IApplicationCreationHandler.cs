namespace JanHafner.Smartbar.Extensibility
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    public interface IApplicationCreationHandler
    {
        Boolean CanCreate([NotNull] Object data);

        [NotNull]
        ICreateApplicationCommand CreateCommand([NotNull] Object data);
    }
}