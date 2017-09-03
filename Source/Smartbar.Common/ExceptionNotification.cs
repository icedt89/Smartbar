namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Text;
    using JetBrains.Annotations;
    using Prism.Events;

    public sealed class ExceptionNotification : PubSubEvent<ExceptionNotification.Data>
    {
        public sealed class Data
        {
            public Data(Exception exception)
                : this(exception, null, null)
            {
            }

            public Data(Exception exception, Object additionalData)
                : this(exception, additionalData, null)
            {
            }

            public Data(Exception exception, String userInfo)
                : this(exception, null, userInfo)
            {
            }

            public Data([NotNull] Exception exception, Object additionalData, String userInformation)
            {
                if (exception == null)
                {
                    throw new ArgumentNullException(nameof(exception));
                }

                this.Exception = exception;
                this.AdditionalData = additionalData;
                this.UserInformation = userInformation;
            }

            [NotNull]
            public Exception Exception { get; private set; }

            [CanBeNull]
            public Object AdditionalData { get; private set; }

            [CanBeNull]
            public String UserInformation { get; private set;}

            public override String ToString()
            {
                var resultBuilder = new StringBuilder();

                resultBuilder.AppendLine("--- An exception was handled ---");
                resultBuilder.AppendLine();

                if (!String.IsNullOrEmpty(this.UserInformation))
                {
                    resultBuilder.AppendLine($"User Information: {this.UserInformation}");
                    resultBuilder.AppendLine();
                }

                if (this.AdditionalData != null)
                {
                    resultBuilder.AppendLine($"Additional Data: {this.AdditionalData}");
                    resultBuilder.AppendLine();
                }

                resultBuilder.AppendLine($"Exception: {this.Exception}");
                resultBuilder.AppendLine();

                return resultBuilder.ToString();
            }
        }
    }
}
