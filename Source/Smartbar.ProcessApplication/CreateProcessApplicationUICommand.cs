namespace JanHafner.Smartbar.ProcessApplication
{
    using System;
    using System.Linq;
    using System.Windows;
    using JanHafner.Smartbar.Common;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.SelectableIconPacks;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource;
    using JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown;
    using JanHafner.Smartbar.Extensibility;
    using JanHafner.Smartbar.Extensibility.BuiltIn;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using JanHafner.Smartbar.Extensibility.UserInterface;
    using JanHafner.Smartbar.ProcessApplication.Commanding;
    using JanHafner.Smartbar.ProcessApplication.EditProcessApplication;
    using JanHafner.Smartbar.Services;
    using JanHafner.Toolkit.Windows.HotKey;
    using JanHafner.Toolkit.Wpf.Behavior;
    using JetBrains.Annotations;
    using Prism.Events;

    internal sealed  class CreateProcessApplicationUICommand : DynamicUICommand
    {
        public CreateProcessApplicationUICommand([NotNull] Func<String> displayTextFactory, [NotNull] IEventAggregator eventAggregator, [NotNull] IWindowService windowService, [NotNull] ICommandDispatcher commandDispatcher,
            [NotNull] IWellKnownIconLibraryUICommandProvider wellKnownIconLibraryUiCommandProvider, [NotNull] ISelectableIconPacksProvider selectableIconPacksProvider, [NotNull] WpfHotKeyManager wpfHotKeyManager, Guid groupId, Int32 column, Int32 row, [NotNull] Func<Boolean> canExecute)
            : base(displayTextFactory, async () =>
            {
                var createProcessApplicationViewModel = new EditProcessApplicationViewModel(windowService);
                if (await windowService.ShowWindowAsync<EditProcessApplication.EditProcessApplication>(createProcessApplicationViewModel) == MessageBoxResult.OK)
                {
                    var applicationId = Guid.NewGuid();
                    var createProcessApplicationContainerCommand = new CreateProcessApplicationContainerCommand
                    {
                        new CreateProcessApplicationCommand(applicationId, createProcessApplicationViewModel.Execute,
                            createProcessApplicationViewModel.Name),
                        new AssignApplicationToGroupCommand(applicationId, groupId),
                        new RepositionApplicationCommand(applicationId, column, row),
                        new UpdateImpersonationCommand(applicationId, createProcessApplicationViewModel.Username,
                            createProcessApplicationViewModel.Password?.ToSecureString()),
                        new UpdateProcessApplicationCommand(applicationId, createProcessApplicationViewModel.Execute,
                            createProcessApplicationViewModel.WorkingDirectory,
                            createProcessApplicationViewModel.Arguments, createProcessApplicationViewModel.SelectedProcessPriorityClass.ProcessPriorityClass, createProcessApplicationViewModel.StretchSmallImage, createProcessApplicationViewModel.SelectedProcessWindowStyle.ProcessWindowStyle)
                    };
                    if (createProcessApplicationViewModel.HotKey != null)
                    {
                        createProcessApplicationContainerCommand.Add(new UpdateProcessApplicationHotKeyCommand(applicationId, (HotKeyModifier)createProcessApplicationViewModel.HotKey.ModifierKeys, createProcessApplicationViewModel.HotKey.Key));
                    }

                    if (!createProcessApplicationViewModel.ProcessAffinityMask.IsSystemAffinityMask)
                    {
                        createProcessApplicationContainerCommand.Add(
                            new SetProcessApplicationProcessAffinityMaskCommand(applicationId,
                                createProcessApplicationViewModel.ProcessAffinityMask.AffinityMask));
                    }

                    createProcessApplicationContainerCommand.TargetColumn = column;
                    createProcessApplicationContainerCommand.TargetRow = row;
                    createProcessApplicationContainerCommand.TargetGroupId = groupId;
                    createProcessApplicationContainerCommand.ApplicationCreateTargetBehavior = ApplicationCreateTargetBehavior.OverrideTarget;

                    var selectNativeResourceViewModel = new SelectNativeResourceViewModel(windowService, eventAggregator,
                        wellKnownIconLibraryUiCommandProvider)
                    {
                        Icon = null
                    };
                    if (await windowService.ShowWindowAsync<SelectNativeResource>(selectNativeResourceViewModel) == MessageBoxResult.OK)
                    {
                        createProcessApplicationContainerCommand.Add((new UpdateApplicationWithImageIconApplicationImageCommand(
                            applicationId, selectNativeResourceViewModel.File,
                            selectNativeResourceViewModel.Icon.Identifier,
                            selectNativeResourceViewModel.Icon.IconIdentifierType)));
                    }
                    else
                    {
                        var selectIconPackResourceViewModel = new SelectIconPackResourceViewModel(windowService, eventAggregator, selectableIconPacksProvider);

                        await selectIconPackResourceViewModel.LoadImagesAsync();
                        if (await windowService.ShowWindowAsync<SelectIconPackResource>(selectIconPackResourceViewModel) != MessageBoxResult.OK)
                        {
                            selectIconPackResourceViewModel.Resource = selectIconPackResourceViewModel.Resources.First();
                        }

                        createProcessApplicationContainerCommand.Add(
                            (new UpdateApplicationWithImageIconPackApplicationImageCommand(applicationId,
                                selectIconPackResourceViewModel.Resource.IconPackType,
                                selectIconPackResourceViewModel.Resource.IconPackKindKey,
                                selectIconPackResourceViewModel.FillColor.ToXaml())));
                    }

                    await commandDispatcher.DispatchAsync(new[] { createProcessApplicationContainerCommand });
                }
            }, canExecute)
        {
        }
    }
}