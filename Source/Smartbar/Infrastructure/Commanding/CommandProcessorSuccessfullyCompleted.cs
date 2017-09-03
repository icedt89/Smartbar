namespace JanHafner.Smartbar.Infrastructure.Commanding
{
    using System.Collections.Generic;
    using JanHafner.Smartbar.Extensibility.Commanding;
    using Prism.Events;

    internal sealed class CommandProcessorSuccessfullyCompleted : PubSubEvent<IEnumerable<ICommand>>
    {
    }
}
