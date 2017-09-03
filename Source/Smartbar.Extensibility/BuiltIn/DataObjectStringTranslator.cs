namespace JanHafner.Smartbar.Extensibility.BuiltIn
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows;
    using JetBrains.Annotations;

    [Export(typeof(IDataObjectTranslator))]
    public class DataObjectStringTranslator : IDataObjectTranslator
    {
        [LinqTunnel]
        public virtual IEnumerable<Object> TranslateData(IDataObject dataObject)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject));
            }

            if (dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                foreach (var fileDrop in (String[])dataObject.GetData(DataFormats.FileDrop))
                {
                    yield return fileDrop;
                }
            }

            if (dataObject.GetDataPresent(DataFormats.StringFormat))
            {
                yield return (String)dataObject.GetData(DataFormats.StringFormat);
            }
        }

        public virtual Boolean CanTranslateData(IDataObject dataObject)
        {
            if (dataObject == null)
            {
                throw new ArgumentNullException(nameof(dataObject));
            }

            return dataObject.GetDataPresent(DataFormats.StringFormat) || dataObject.GetDataPresent(DataFormats.FileDrop);
        }
    }
}
