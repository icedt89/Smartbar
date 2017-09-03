namespace JanHafner.Smartbar.Views.ModuleExplorer
{
    using System;
    using JetBrains.Annotations;
    using Prism.Mvvm;

    internal sealed class ModuleViewModel : BindableBase
    {
        public ModuleViewModel([NotNull] Type module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            this.ModuleType= module;
        }

        public String Name
        {
            get
            {
                return this.ModuleType.Name;
            }
        }

        public String File
        {
            get
            {
                return this.ModuleType.Assembly.Location.ToLower();
            }
        }

        [NotNull]
        internal Type ModuleType { get; private set; }
    }
}
