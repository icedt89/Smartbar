namespace JanHafner.Smartbar.Extensibility
{
    using System;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    public interface IApplicationExecutionHandler
    {
        Boolean CanExecute([NotNull] Application application);

        void Execute([NotNull] Application application);
    }
}
