namespace Smartbar.Updater.Core
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    public interface ISmartbarUpdater
    {
        Task<Update> GetUpdateAsync();

        Task UpdateRemoteInformationAsync([NotNull] Update update);
    }
}
