namespace JanHafner.Smartbar.Infrastructure.Composition
{
    using System;
    using System.Collections.Generic;

    internal interface IModuleExplorer
    {
        IEnumerable<Type> GetImportedTypes();
    }
}
