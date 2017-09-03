namespace JanHafner.Smartbar.Common.Localization
{
    using System;
    using System.Collections.Generic;
    using System.Resources;
    using System.Windows;
    using System.Windows.Markup;
    using JetBrains.Annotations;

    [MarkupExtensionReturnType(typeof(String))]
    public sealed class ResX : MarkupExtension
    {
        /// <summary>
        /// Cached instances of <see cref="ResourceManager"/> instances. Speeds up whole resource lookup process significant.
        /// </summary>
        private static readonly IDictionary<Type, ResourceManager> cachedResourceManagers = new Dictionary<Type, ResourceManager>();

        // Otherwise, XAML/Resharper complains about missing parameterless constructor.
        public ResX()
        {
        }

        public ResX(String resourceName)
            : this()
        {
            if (String.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }

            this.ResourceName = resourceName;
        }

        [CanBeNull]
        public Type ResourceType { get; set; }

        [CanBeNull]
        public String ResourceName { get; set; }

        public override Object ProvideValue([CanBeNull] IServiceProvider serviceProvider)
        {
            if (this.ResourceType == null)
            {
                throw new InvalidOperationException("ResourceType must be specified.");
            }

            if (String.IsNullOrWhiteSpace(this.ResourceName))
            {
                throw new InvalidOperationException("ResourceName must be specified.");
            }

            var provideValueTarget = serviceProvider?.GetService(typeof (IProvideValueTarget)) as IProvideValueTarget;
            if (provideValueTarget != null)
            {
                // Is returned if the MarkupExtension is used inside a template.
                // Return the MarkupExtension itself to instruct WPF to recall the MarkupExtension when the control is created.
                if (provideValueTarget.TargetObject.GetType().Name == "SharedDp")
                {
                    return this;
                }

                this.TryBindAutomaticTargetUpdate(provideValueTarget);
            }

            return this.GetLocalizedResourceString();
        }

        /// <summary>
        /// Binds an update mechanism to the bound control, so if the UICulture is changed, the localization is propagated to the bound control.
        /// </summary>
        /// <returns>A vlaue, indicating whether, the binding was successful.</returns>
        private void TryBindAutomaticTargetUpdate([NotNull] IProvideValueTarget provideValueTarget)
        {
            if (provideValueTarget == null)
            {
                throw new ArgumentNullException(nameof(provideValueTarget));
            }

            var dependencyObject = provideValueTarget.TargetObject as DependencyObject;
            var dependencyProperty = provideValueTarget.TargetProperty as DependencyProperty;
            if (dependencyObject == null || dependencyProperty == null)
            {
                return;
            }

            LocalizationService.Current.CurrentUICultureChanged += (s, e) =>
            {
                var localizedResourceString = this.GetLocalizedResourceString();

                dependencyObject.SetValue(dependencyProperty, localizedResourceString);
            };
        }

        private String GetLocalizedResourceString()
        {
            return LocalizationService.Current.Localize(this.ResourceType, this.ResourceName);
        }
    }
}
