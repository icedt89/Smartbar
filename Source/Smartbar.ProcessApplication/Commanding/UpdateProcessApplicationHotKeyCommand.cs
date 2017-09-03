namespace JanHafner.Smartbar.ProcessApplication.Commanding
{
    using System;
    using System.Windows.Input;
    using JanHafner.Toolkit.Windows.HotKey;
    using ICommand = JanHafner.Smartbar.Extensibility.Commanding.ICommand;

    public sealed class UpdateProcessApplicationHotKeyCommand : ICommand
    {
        public UpdateProcessApplicationHotKeyCommand(Guid applicationId, HotKeyModifier hotKeyModifier, Key hotKey)
        {
            this.ApplicationId = applicationId;
            this.HotKeyModifier = hotKeyModifier;
            this.HotKey = hotKey;
        }

        public Guid ApplicationId { get; private set; }

        public HotKeyModifier HotKeyModifier { get; private set; }

        public Key HotKey { get; private set; }
    }
}
