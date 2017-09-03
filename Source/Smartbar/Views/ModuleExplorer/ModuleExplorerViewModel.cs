namespace JanHafner.Smartbar.Views.ModuleExplorer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common.UserInterface.Dialogs;
    using JanHafner.Smartbar.Infrastructure.Composition;
    using JanHafner.Smartbar.Services;
    using JetBrains.Annotations;
    using Prism.Mvvm;

    internal sealed class ModuleExplorerViewModel : BindableBase
    {
        [NotNull]
        private readonly IWindowService windowService;

        public ModuleExplorerViewModel([NotNull] IWindowService windowService, [NotNull] IModuleExplorer moduleExplorer)
        {
            if (windowService == null)
            {
                throw new ArgumentNullException(nameof(windowService));
            }

            if (moduleExplorer == null)
            {
                throw new ArgumentNullException(nameof(moduleExplorer));
            }

            this.windowService = windowService;

            this.Modules = moduleExplorer.GetImportedTypes().Select(t => new ModuleViewModel(t)).OrderBy(m => m.File).ThenBy(m => m.Name).ToList();
        }

        [NotNull]
        public IEnumerable<ModuleViewModel> Modules { get; private set; }

        [NotNull]
        public ICommand OKCommand
        {
            get
            {
                return new CommonOKCommand<ModuleExplorerViewModel>(this, this.windowService);
            }
        }

        [NotNull]
        public ICommand ExportLoadedModulesCommand
        {
            get { return new ExportLoadedModulesCommand(this.Modules, this.windowService); }
        }
    }
}
