namespace JanHafner.Smartbar.Extensibility
{
    public interface IApplicationLifecycleObserver
    {
        void AfterInitialization();

        void BeforeShutdown();
    }
}