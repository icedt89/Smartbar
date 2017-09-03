namespace JanHafner.Smartbar.Extensibility
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using JetBrains.Annotations;

    public interface IDataObjectTranslator
    {
        [LinqTunnel]
        [NotNull]
        IEnumerable<Object> TranslateData([NotNull] IDataObject dataObject);

        Boolean CanTranslateData([NotNull] IDataObject dataObject);
    }
}
