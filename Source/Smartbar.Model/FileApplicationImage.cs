namespace JanHafner.Smartbar.Model
{
    using System;
    using JetBrains.Annotations;

    public sealed class FileApplicationImage : ApplicationImage, ICloneable
    {
        private FileApplicationImage()
        {
        }

        public FileApplicationImage([NotNull] String file)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(nameof(file));
            }

            this.File = file;
        }

        public String File { get; private set; }

        public override void Update(ApplicationImage applicationImage)
        {
            if (applicationImage == null)
            {
                throw new ArgumentNullException(nameof(applicationImage));
            }

            var fileApplicationImage = (FileApplicationImage)applicationImage;
            this.File = fileApplicationImage.File;
        }

        public Object Clone()
        {
            return new FileApplicationImage(this.File);
        }
    }
}
