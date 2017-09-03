namespace JanHafner.Smartbar.Common
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using JanHafner.Smartbar.Common.Properties;
    using JetBrains.Annotations;

    [Serializable]
    public sealed class ExplorerExeNotFoundException : FileNotFoundException
    {
        public ExplorerExeNotFoundException([NotNull] String explorerExeName, [NotNull] String directory)
            : base(String.Format(ExceptionMessages.ExplorerExeNotFoundException, explorerExeName))
        {
            if (String.IsNullOrWhiteSpace(explorerExeName))
            {
                throw new ArgumentNullException(nameof(explorerExeName));
            }

            if (String.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory));
            }

            this.ExplorerExeName = explorerExeName;
            this.Directory = directory;
        }

        private ExplorerExeNotFoundException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        [NotNull]
        public String ExplorerExeName { get; private set; }

        [NotNull]
        public String Directory { get; private set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("ExplorerExeName", this.ExplorerExeName);
            info.AddValue("Directory", this.Directory);

            base.GetObjectData(info, context);
        }
    }
}
