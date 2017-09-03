namespace JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource
{
    using System;
    using System.Windows;
    using JanHafner.Smartbar.Common.UserInterface.SelectIconPackResource.Loading;
    using JetBrains.Annotations;
    using MahApps.Metro.IconPacks;

    internal sealed class IconPackDataTemplateSelector : System.Windows.Controls.DataTemplateSelector
    {
        [NotNull]
        public DataTemplate ModernDataTemplate { get; set; }

        [NotNull]
        public DataTemplate MaterialDataTemplate { get; set; }

        [NotNull]
        public DataTemplate EntypoDataTemplate { get; set; }

        [NotNull]
        public DataTemplate FontAwesomeDatatemplate { get; set; }

        public DataTemplate OcticonsAwesomeDatatemplate { get; set; }

        public override DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            var iconPackResourceBag = (IconPackResourceBag)item;
            if (iconPackResourceBag.IconPackType == typeof (PackIconModern))
            {
                return this.ModernDataTemplate;
            }

            if (iconPackResourceBag.IconPackType == typeof(PackIconFontAwesome))
            {
                return this.FontAwesomeDatatemplate;
            }

            if (iconPackResourceBag.IconPackType == typeof(PackIconMaterial))
            {
                return this.MaterialDataTemplate;
            }

            if (iconPackResourceBag.IconPackType == typeof(PackIconEntypo))
            {
                return this.EntypoDataTemplate;
            }

            if (iconPackResourceBag.IconPackType == typeof(PackIconOcticons))
            {
                return this.OcticonsAwesomeDatatemplate;
            }

            throw new InvalidOperationException();
        }
    }
}
