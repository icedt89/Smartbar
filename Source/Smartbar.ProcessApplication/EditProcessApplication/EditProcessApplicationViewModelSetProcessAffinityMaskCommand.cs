namespace JanHafner.Smartbar.ProcessApplication.EditProcessApplication
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.SetProcessAffinityMask;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Windows;
    using Prism.Commands;

    internal sealed class EditProcessApplicationViewModelSetProcessAffinityMaskCommand : DelegateCommand
    {
        public EditProcessApplicationViewModelSetProcessAffinityMaskCommand(EditProcessApplicationViewModel editProcessApplicationViewModel, IWindowService windowService)
            : base(async () =>
            {
                var processAffinityMask = editProcessApplicationViewModel.ProcessAffinityMask ?? ProcessAffinityMask.ForCurrentProcess();
                var setProcessAffinityMaskViewModel = new SetProcessAffinityMaskViewModel(processAffinityMask, windowService);
                switch (await windowService.ShowWindowAsync<SetProcessAffinityMask>(setProcessAffinityMaskViewModel))
                {
                    case MessageBoxResult.OK:
                        editProcessApplicationViewModel.ProcessAffinityMask = ProcessAffinityMask.FromAffinityMask((UInt32)setProcessAffinityMaskViewModel.AffinityMask);
                        break;
                    case MessageBoxResult.No:
                        editProcessApplicationViewModel.ProcessAffinityMask = null;
                        break;
                }
            })
        {
        }
    }
}
