namespace JanHafner.Smartbar.Common.Validation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;
    using Prism.Mvvm;

    /// <summary>
    /// Original mplementation from http://blog.pluralsight.com/async-validation-wpf-prism by Brian Noyes.
    /// </summary>
    public abstract class ValidatableBindableBase : BindableBase, INotifyDataErrorInfo
    {
        [NotNull]
        private readonly ErrorsContainer<String> errorsContainer;

        [NotNull]
        private readonly ChangeContainer changeContainer;

        protected ValidatableBindableBase()
        {
            this.errorsContainer = new ErrorsContainer<String>(propertyName => this.OnErrorsChanged(new DataErrorsChangedEventArgs(propertyName)));
            this.changeContainer = new ChangeContainer();
            this.ValidateAlwaysAllPropertiesOnChange = true;
        }

        #region ErrorsChanged

        private EventHandler<DataErrorsChangedEventArgs> errorsChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged
        {
            add { this.errorsChanged += value; }
            remove { this.errorsChanged -= value; }
        }

        private void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            this.errorsChanged?.Invoke(this, e);
        }

        #endregion

        protected Boolean ValidateAlwaysAllPropertiesOnChange { get; set; }

        public Boolean IsChanged
        {
            get
            {
                return this.changeContainer.IsChanged;
            }
        }

        public Boolean HasErrors
        {
            get
            {
                return this.errorsContainer.HasErrors;
            }
        }

        [NotNull]
        public IEnumerable GetErrors(String propertyName)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return this.errorsContainer.GetErrors(propertyName);
        }

        protected override Boolean SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (String.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            this.changeContainer.SetChange(storage, value, propertyName);
            var result = base.SetProperty(ref storage, value, propertyName);

            if (this.ValidateAlwaysAllPropertiesOnChange)
            {
                this.ValidateAllProperties();
            }
            else
            {
                this.ValidateProperty(propertyName);
            }

            this.OnPropertyChanged(() => this.IsChanged);
            
            return result;
        }

        private Boolean ValidateProperty([NotNull, CallerMemberName] String propertyName = null)
        {
            var propertyInfo = this.GetType().GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new ArgumentException("Invalid property name", propertyName);
            }

            var propertyErrors = new List<String>();
            var isValid = this.TryValidateProperty(propertyInfo, propertyErrors);

            this.errorsContainer.SetErrors(propertyInfo.Name, propertyErrors);

            return isValid;
        }

        protected Boolean ValidateAllProperties()
        {
            var validatableProperties = this.GetType()
                        .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(_ => _.GetCustomAttributes<ValidationAttribute>().Any() || this.customValidatorCollection.ContainsKey(_.Name))
                        .Select(property => new
                        {
                            CurrentValue = property.GetValue(this),
                            Property = property
                        });

            var result = true;
            foreach (var validatableProperty in validatableProperties)
            {
                List<ValidationResult> validationResults;
                var validationResult = this.TryValidatePropertyCore(validatableProperty.Property, validatableProperty.CurrentValue, out validationResults);

                this.errorsContainer.SetErrors(validatableProperty.Property.Name, validationResults.Select(propertyError => propertyError.ErrorMessage));

                if (!validationResult && validationResults.Any())
                {
                    result = false;
                }
            }

            return result;
        }

        private Boolean TryValidateProperty([NotNull] PropertyInfo propertyInfo, [NotNull] List<String> propertyErrors)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (propertyErrors == null)
            {
                throw new ArgumentNullException(nameof(propertyErrors));
            }

            var currentValue = propertyInfo.GetValue(this);

            List<ValidationResult> validationResults;
            var validationResult = this.TryValidatePropertyCore(propertyInfo, currentValue, out validationResults);

            if (!validationResult && validationResults.Any())
            {
                propertyErrors.AddRange(validationResults.Select(propertyError => propertyError.ErrorMessage));
            }

            return validationResult;
        }

        private readonly IDictionary<String, ICollection<Action<Object, ICollection<ValidationResult>>>> customValidatorCollection = new Dictionary<String, ICollection<Action<Object, ICollection<ValidationResult>>>>();

        protected void AddCustomPropertyValidation(String propertyName,
            Action<Object, ICollection<ValidationResult>> validator)
        {
            ICollection<Action<Object, ICollection<ValidationResult>>> customValidators;
            if (!this.customValidatorCollection.TryGetValue(propertyName, out customValidators))
            {
                customValidators = new List<Action<Object, ICollection<ValidationResult>>>();
                this.customValidatorCollection[propertyName] = customValidators;
            }

            this.customValidatorCollection[propertyName].Add(validator);
        }


        private Boolean TryValidatePropertyCore([NotNull] PropertyInfo propertyInfo, Object currentValue, [NotNull] out List<ValidationResult> propertyErrors)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            propertyErrors = new List<ValidationResult>();

            var results = new List<ValidationResult>();
            var context = new ValidationContext(this)
            {
                MemberName = propertyInfo.Name
            };

            ICollection<Action<Object, ICollection<ValidationResult>>> customValidators;
            if (this.customValidatorCollection.TryGetValue(propertyInfo.Name, out customValidators))
            {
                customValidators.ForEach(_ => _(currentValue, results));   
            }

            var isValid = Validator.TryValidateProperty(currentValue, context, results);

            if (!isValid || results.Any())
            {
                propertyErrors = results;
            }

            return isValid;
        }
    }
}
