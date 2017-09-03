namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System.Collections.Generic;
    using System.Linq;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;

    public sealed class CreateProcessApplicationContainerCommand : CreateApplicationContainerCommand
    {
        public CreateProcessApplicationCommand CreateProcessApplicationCommand
        {
            get { return (CreateProcessApplicationCommand) this.Single(c => c is CreateProcessApplicationCommand); }
        }

        public IEnumerable<UpdateProcessApplicationCommand> UpdateProcessApplicationCommands
        {
            get { return this.OfType<UpdateProcessApplicationCommand>(); }
        }

        public IEnumerable<IUpdateApplicationImageCommand> UpdateProcessApplicationImageCommands
        {
            get { return this.OfType<IUpdateApplicationImageCommand>(); }
        }

        public IEnumerable<UpdateImpersonationCommand> UpdateImpersonationCommands
        {
            get { return this.OfType<UpdateImpersonationCommand>(); }
        }

        public IEnumerable<UpdateProcessApplicationHotKeyCommand> UpdateHotKeyCommands
        {
            get { return this.OfType<UpdateProcessApplicationHotKeyCommand>(); }
        }

        public IEnumerable<SetProcessApplicationProcessAffinityMaskCommand> SetProcessAffinityMaskCommands
        {
            get { return this.OfType<SetProcessApplicationProcessAffinityMaskCommand>(); }
        }
    }
}
