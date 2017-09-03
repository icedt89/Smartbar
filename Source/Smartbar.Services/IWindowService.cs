using System;
using System.Threading.Tasks;
using System.Windows;

namespace JanHafner.Smartbar.Services
{
    using JetBrains.Annotations;

    public interface IWindowService
    {
        void CloseWindow([NotNull] Object viewModel, MessageBoxResult result);

        Task<MessageBoxResult> ShowWindowAsync<TWindow>([NotNull] Object viewModel) where TWindow : Window;

        Task ShowWindowAsync<TWindow>([NotNull] Object viewModel, [NotNull] Action<MessageBoxResult> closedCallback) where TWindow : Window;
    }
}