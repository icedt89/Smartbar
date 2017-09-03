namespace JanHafner.Smartbar.Extensibility
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class HandlerOverrideAttribute : Attribute
    {
        public HandlerOverrideAttribute([NotNull] Type fullHandlerType)
        {
            if (fullHandlerType == null)
            {
                throw new ArgumentNullException(nameof(fullHandlerType));
            }

            this.FullHandlerType = fullHandlerType.FullName;
        }

        [NotNull]
        public String FullHandlerType { get; private set; }
    }
}
