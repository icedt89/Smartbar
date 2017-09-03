namespace JanHafner.Smartbar.Extensibility.Commanding.Events
{
    using System.Globalization;
    using Prism.Events;

    public sealed class CurrentLanguageChanged : PubSubEvent<CultureInfo>
    {
    }
}
