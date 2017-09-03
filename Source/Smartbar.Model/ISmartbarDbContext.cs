namespace JanHafner.Smartbar.Model
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ISmartbarDbContext
    {
        ICollection<Group> Groups { get; }

        ICollection<PluginConfiguration> PluginConfigurations { get; }

        Task SaveChangesAsync();
    }
}