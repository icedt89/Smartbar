namespace JanHafner.Smartbar.ProcessApplication.ProcessApplicationButton
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;
    using MahApps.Metro.IconPacks;

    internal sealed class ImageVisualizationDataTemplateSelector : DataTemplateSelector
    {
        [NotNull]
        public DataTemplate IconPackModernVisualizationDataTemplate { get; set; }

        [NotNull]
        public DataTemplate IconPackMaterialVisualizationDataTemplate { get; set; }

        [NotNull]
        public DataTemplate IconPackEntypoVisualizationDataTemplate { get; set; }

        [NotNull]
        public DataTemplate IconPackFontAwesomeVisualizationDataTemplate { get; set; }

        public DataTemplate IconPackOcticonsVisualizationDataTemplate { get; set; }

        [NotNull]
        public DataTemplate DefaultImageVisualizationHandlerDataTemplate { get; set; }

        private Boolean propertyChangedIsAttached;

        public override DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var application = (ProcessApplicationButtonViewModel)item;

            if (!this.propertyChangedIsAttached)
            {
                application.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == nameof(ProcessApplicationButtonViewModel.ApplicationImage))
                    {
                        var contentPresenter = (ContentPresenter)container;
                        contentPresenter.ContentTemplateSelector = null;
                        contentPresenter.ContentTemplateSelector = this;
                    }
                };

                this.propertyChangedIsAttached = true;
            }

            var iconPackApplicationImage = application.ApplicationImage as IconPackApplicationImage;
            if (iconPackApplicationImage != null)
            {
                if (iconPackApplicationImage.IconPackType == typeof(PackIconModern))
                {
                    return this.IconPackModernVisualizationDataTemplate;
                }

                if (iconPackApplicationImage.IconPackType == typeof(PackIconFontAwesome))
                {
                    return this.IconPackFontAwesomeVisualizationDataTemplate;
                }

                if (iconPackApplicationImage.IconPackType == typeof(PackIconMaterial))
                {
                    return this.IconPackMaterialVisualizationDataTemplate;
                }

                if (iconPackApplicationImage.IconPackType == typeof(PackIconEntypo))
                {
                    return this.IconPackEntypoVisualizationDataTemplate;
                }

                if (iconPackApplicationImage.IconPackType == typeof(PackIconOcticons))
                {
                    return this.IconPackOcticonsVisualizationDataTemplate;
                }

                throw new InvalidOperationException();
            }

            return this.DefaultImageVisualizationHandlerDataTemplate;
        }
    }
}
