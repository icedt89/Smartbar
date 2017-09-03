namespace JanHafner.Smartbar.Common.UserInterface.SetProcessAffinityMask.EditConfiguration
{
    using System;
    using JanHafner.Smartbar.Model;

    public sealed class ProcessAffinityMaskDialogConfiguration : PluginConfiguration
    {
        public ProcessAffinityMaskDialogConfiguration()
        {
            this.ShowOnlyAvailableProcessors = true;
        }

        public Boolean ShowOnlyAvailableProcessors { get; internal set; }
    }
}
