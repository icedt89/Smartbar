namespace JanHafner.Smartbar.Common.UserInterface.SelectNativeResource.WellKnown
{
    using System;
    using JetBrains.Annotations;

    public abstract class WellKnownIconLibrary : IWellKnownIconLibrary
    {
        protected WellKnownIconLibrary([NotNull] String file, [NotNull] String displayTextResourceName)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (String.IsNullOrEmpty(displayTextResourceName))
            {
                throw new ArgumentNullException(nameof(displayTextResourceName));
            }

            this.DisplayTextResourceName = displayTextResourceName;
            this.File = file;
            this.IsAvailable = System.IO.File.Exists(file);
        }

        public String File { get; private set; }

        public String DisplayTextResourceName { get; private set; }

        public Boolean IsAvailable { get; private set; }
    }
}
