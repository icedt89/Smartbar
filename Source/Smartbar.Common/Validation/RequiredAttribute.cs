namespace JanHafner.Smartbar.Common.Validation
{
    using System;
    using Localization;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public override String FormatErrorMessage(String name)
        {
            if (String.IsNullOrEmpty(name) || this.ErrorMessageResourceType == null)
            {
                return String.Empty;
            }

            // Fixes a (in my opinion!) bug where the ValidationAttribute does not actually localize the error message!
            // The ValidationAttribute does just call the static property, named by ErrorMessageResourceName, on type ErrorMessageResourceType.
            // Thats really stupid Microsoft :)
            return LocalizationService.Current.Localize(this.ErrorMessageResourceType, this.ErrorMessageResourceName);
        }
    }
}
