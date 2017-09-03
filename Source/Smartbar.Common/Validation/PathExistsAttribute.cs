namespace JanHafner.Smartbar.Common.Validation
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using JanHafner.Smartbar.Common.Localization;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class PathExistsAttribute : ValidationAttribute
    {
        public PathExistsAttribute()
        {
            this.TreatNullAsValid = true;
            this.TreatEmptyStringAsValid = true;
            this.PathValidationType = PathValidationType.Both;
        }

        public Boolean TreatNullAsValid { get; set; }

        public Boolean TreatEmptyStringAsValid { get; set; }

        public PathValidationType PathValidationType { get; set; }

        public override String FormatErrorMessage(String name)
        {
            // Fixes a (in my opinion!) bug where the ValidationAttribute does not actually localize the error message!
            // The ValidationAttribute does just call the static property, named by ErrorMessageResourceName, on type ErrorMessageResourceType.
            // Thats really stupid Microsoft :)
            return LocalizationService.Current.Localize(this.ErrorMessageResourceType, this.ErrorMessageResourceName);
        }

        public override Boolean IsValid(Object value)
        {
            if (value == null)
            {
                return this.TreatNullAsValid;
            }

            var path = value.ToString();
            if (String.IsNullOrEmpty(path))
            {
                return this.TreatEmptyStringAsValid;
            }

            switch (this.PathValidationType)
            {
                case PathValidationType.Directory:
                    return Directory.Exists(path);
                case PathValidationType.File:
                    return File.Exists(path);
                case PathValidationType.Both:
                    return File.Exists(path) || Directory.Exists(path);
                default:
                    throw new InvalidOperationException($"Invalid 'PathValidationType' with value '{this.PathValidationType}' supplied.");
            }
        }
    }
}
