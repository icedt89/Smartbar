namespace JanHafner.Smartbar.ProcessApplication.ProcessApplicationButton
{
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Input;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.ProcessApplication.EditProcessApplication;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Windows.HotKey;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Commands;
    using ICommand = JanHafner.Smartbar.Extensibility.Commanding.ICommand;

    internal sealed class ApplicationViewModelEditApplicationCommand : DelegateCommand
    {
        public ApplicationViewModelEditApplicationCommand([NotNull] ProcessApplicationButtonViewModel processApplicationViewModel, [NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher, [NotNull] ISmartbarService smartbarService, [NotNull] WpfHotKeyManager wpfHotKeyManager)
            : base(async () =>
            {
                var processApplication = smartbarService.GetApplication<ProcessApplication>(processApplicationViewModel.Id);
                var editProcessApplicationViewModel = new EditProcessApplicationViewModel(processApplication, windowService);
                if (await windowService.ShowWindowAsync<EditProcessApplication>(editProcessApplicationViewModel) == MessageBoxResult.OK)
                {
                    var commands = new List<ICommand>
                    {
                        new UpdateProcessApplicationCommand(processApplicationViewModel.Id,
                            editProcessApplicationViewModel.Execute, editProcessApplicationViewModel.WorkingDirectory,
                            editProcessApplicationViewModel.Arguments,
                            editProcessApplicationViewModel.SelectedProcessPriorityClass.ProcessPriorityClass,
                            editProcessApplicationViewModel.StretchSmallImage,
                            editProcessApplicationViewModel.SelectedProcessWindowStyle.ProcessWindowStyle),
                        new RenameProcessApplicationCommand(processApplicationViewModel.Id,
                            editProcessApplicationViewModel.Name),
                        new UpdateImpersonationCommand(processApplicationViewModel.Id,
                            editProcessApplicationViewModel.Username, editProcessApplicationViewModel.Password?.ToSecureString()),
                        new SetProcessApplicationProcessAffinityMaskCommand(processApplicationViewModel.Id,
                            editProcessApplicationViewModel.ProcessAffinityMask?.AffinityMask),
                        editProcessApplicationViewModel.HotKey != null
                            ? new UpdateProcessApplicationHotKeyCommand(processApplication.Id,
                                (HotKeyModifier) editProcessApplicationViewModel.HotKey.ModifierKeys,
                                editProcessApplicationViewModel.HotKey.Key)
                            : new UpdateProcessApplicationHotKeyCommand(processApplication.Id, HotKeyModifier.None,
                                Key.None)
                    };

                    if (!editProcessApplicationViewModel.ProcessAffinityMask.IsSystemAffinityMask)
                    {
                        commands.Add(
                            new SetProcessApplicationProcessAffinityMaskCommand(processApplicationViewModel.Id,
                                editProcessApplicationViewModel.ProcessAffinityMask.AffinityMask));
                    }
                    else
                    {
                        commands.Add(new SetProcessApplicationProcessAffinityMaskCommand(processApplicationViewModel.Id, null));
                    }

                    await commandDispatcher.DispatchAsync(commands);
                }
            })
        {
        }
    }
}
