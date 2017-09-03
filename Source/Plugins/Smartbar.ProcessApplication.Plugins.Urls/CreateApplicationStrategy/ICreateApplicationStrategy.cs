namespace JanHafner.Smartbar.ProcessApplication.Plugins.Urls.CreateApplicationStrategy
{
    using System;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JetBrains.Annotations;

    internal interface ICreateApplicationStrategy
    {
        [NotNull]
        CreateApplicationContainerCommand CreateContainerCommand([NotNull] String data);
    }
}