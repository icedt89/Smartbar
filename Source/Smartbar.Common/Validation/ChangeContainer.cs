namespace JanHafner.Smartbar.Common.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public sealed class ChangeContainer
    {
        private readonly IDictionary<String, ChangeInfo> changes;

        public ChangeContainer()
        {
            this.changes = new Dictionary<String, ChangeInfo>();
        }

        public Boolean SetChange(Object currentValue, Object newValue, [CallerMemberName] String propertyName = null)
        {
            ChangeInfo changeInfo;
            if (this.changes.TryGetValue(propertyName, out changeInfo))
            {
                changeInfo.CurrentValue = newValue;
            }
            else
            {
                changeInfo = new ChangeInfo(currentValue)
                {
                    CurrentValue = newValue
                };
                this.changes.Add(propertyName, changeInfo);
            }

            return changeInfo.IsChanged;
        }

        public Boolean IsChanged
        {
            get
            {
                return this.changes.Values.Any(change => change.IsChanged);
            }
        }

        private sealed class ChangeInfo
        {
            public ChangeInfo(Object originalValue)
            {
                this.OriginalValue = originalValue;
            }

            public Object OriginalValue { get; private set; }

            public Object CurrentValue { get; set; }

            public Boolean IsChanged
            {
                get
                {
                    var comparableOriginalValue = this.OriginalValue;
                    if (comparableOriginalValue is String && (String) comparableOriginalValue == "")
                    {
                        comparableOriginalValue = null;
                    }

                    var comparableCurrentValue = this.CurrentValue;
                    if (comparableCurrentValue is String && (String)comparableCurrentValue == "")
                    {
                        comparableCurrentValue = null;
                    }

                    return !Object.Equals(comparableOriginalValue, comparableCurrentValue);
                }
            }
        }
    }
}
