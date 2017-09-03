namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using JetBrains.Annotations;

    public interface IWellKnownIconLibrary
    {
        [NotNull]
        String DisplayTextResourceName { get; }

        [NotNull]
        String File { get; }

        Boolean IsAvailable { get; }
    }
}