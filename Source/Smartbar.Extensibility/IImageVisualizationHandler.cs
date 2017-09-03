namespace JanHafner.Smartbar.Extensibility
{
    using System;
    using System.Windows;
    using System.Windows.Media;
    using JanHafner.Smartbar.Model;
    using JetBrains.Annotations;

    public interface IImageVisualizationHandler
    {   
        Boolean CanVisualize([NotNull] ApplicationImage applicationImage);

        [CanBeNull]
        ImageSource Visualize([NotNull] ApplicationImage applicationImage, Size desiredSize);
    }
}
