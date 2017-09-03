namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using Prism.Events;

    public sealed class ApplicationImpersonationUpdated : PubSubEvent<Guid>
    {
    }
}