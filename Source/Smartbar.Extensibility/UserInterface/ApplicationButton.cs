namespace JanHafner.Smartbar.Extensibility.UserInterface
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using JanHafner.Toolkit.Common.ExtensionMethods;
    using JetBrains.Annotations;

    public abstract class ApplicationButton : Button
    {
        protected ApplicationButton()
        {
            // Apply default style to all inheritors.
            // DefaultStyleKeyProperty.Override(...) does not work... :(
            this.SetResourceReference(StyleProperty, typeof(ApplicationButton));

            this.Unloaded += (sender, args) => this.DataContext.ToDisposable().Dispose();
        }

        #region IsPotentialDropTarget

        public static readonly DependencyProperty IsPotentialDropTargetProperty = DependencyProperty.Register(
            "IsPotentialDropTarget", typeof(Boolean), typeof(ApplicationButton), new PropertyMetadata(default(Boolean)));

        public Boolean IsPotentialDropTarget
        {
            get { return (Boolean) this.GetValue(IsPotentialDropTargetProperty); }
            set { this.SetValue(IsPotentialDropTargetProperty, value); }
        }

        #endregion

        #region SomethingIsWrong

        public static readonly DependencyProperty SomethingIsWrongProperty = DependencyProperty.Register(
            "SomethingIsWrong", typeof (Boolean), typeof (ApplicationButton), new PropertyMetadata(default(Boolean)));

        public Boolean SomethingIsWrong
        {
            get { return (Boolean) this.GetValue(SomethingIsWrongProperty); }
            set { this.SetValue(SomethingIsWrongProperty, value); }
        }

        #endregion

        #region IsSelected

        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register(
            "IsSelected", typeof (Boolean), typeof (ApplicationButton), new PropertyMetadata(default(Boolean)));

        public Boolean IsSelected
        {
            get { return (Boolean) this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }

        #endregion

        #region IsSelectable

        public static readonly DependencyProperty IsSelectableProperty = DependencyProperty.Register(
            "IsSelectable", typeof(Boolean), typeof(ApplicationButton), new PropertyMetadata(true));

        public Boolean IsSelectable
        {
            get { return (Boolean) this.GetValue(IsSelectableProperty); }
            set { this.SetValue(IsSelectableProperty, value); }
        }

        #endregion

        #region IsPressable

        public static readonly DependencyProperty IsPressableProperty = DependencyProperty.Register(
            "IsPressable", typeof(Boolean), typeof(ApplicationButton), new PropertyMetadata(true));

        public Boolean IsPressable
        {
            get { return (Boolean)this.GetValue(IsPressableProperty); }
            set { this.SetValue(IsPressableProperty, value); }
        }

        protected override void OnIsPressedChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!this.IsPressable)
            {
                this.IsPressed = false;
            }

            base.OnIsPressedChanged(e);
        }

        public void Unpress()
        {
            this.IsPressed = false;
        }

        #endregion

        #region IsDragSource

        public static readonly DependencyProperty IsDragSourceProperty = DependencyProperty.Register(
            "IsDragSource", typeof(Boolean), typeof(ApplicationButton), new PropertyMetadata(true));

        public Boolean IsDragSource
        {
            get { return (Boolean) this.GetValue(IsDragSourceProperty); }
            set { this.SetValue(IsDragSourceProperty, value); }
        }

        #endregion

        #region ApplicationId

        public static readonly DependencyProperty ApplicationIdProperty = DependencyProperty.Register(
            "ApplicationId", typeof(Guid), typeof(ApplicationButton), new PropertyMetadata(default(Guid)));

        public Guid ApplicationId
        {
            get { return (Guid) this.GetValue(ApplicationIdProperty); }
            set { this.SetValue(ApplicationIdProperty, value); }
        }

        #endregion

        #region Preview

        #region IsPreviewing

        private static readonly DependencyPropertyKey IsPreviewingPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsPreviewing", typeof(Boolean), typeof(ApplicationButton), new PropertyMetadata(false));

        private static readonly DependencyProperty IsPreviewingProperty = IsPreviewingPropertyKey.DependencyProperty;

        public Boolean IsPreviewing
        {
            get { return (Boolean) this.GetValue(IsPreviewingProperty); }
            private set { this.SetValue(IsPreviewingPropertyKey, value); }
        }

        #endregion

        #region PreviewEnabled

        public static readonly DependencyProperty PreviewEnabledProperty = DependencyProperty.Register(
            "PreviewEnabled", typeof(Boolean), typeof(ApplicationButton), new PropertyMetadata(true, PreviewEnabledChangedCallback));

        private static void PreviewEnabledChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var applicationButton = (ApplicationButton)dependencyObject;
            if ((Boolean) dependencyPropertyChangedEventArgs.NewValue && applicationButton.PreviewTemplate != null && applicationButton.PreviewDataContext != null)
            {
                applicationButton.ApplyPreviewableContent();
            }
            else
            {
                applicationButton.ReapplyDefaultContent();
            }
        }

        public Boolean PreviewEnabled
        {
            get { return (Boolean) this.GetValue(PreviewEnabledProperty); }
            set { this.SetValue(PreviewEnabledProperty, value); }
        }

        #endregion

        #region PreviewTemplate

        public static readonly DependencyProperty PreviewTemplateProperty = DependencyProperty.Register(
            "PreviewTemplate", typeof(DataTemplate), typeof(ApplicationButton), new PropertyMetadata(default(DataTemplate), PreviewTemplateChangedCallback));

        private static void PreviewTemplateChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var applicationButton = (ApplicationButton)dependencyObject;
            if (applicationButton.PreviewEnabled && dependencyPropertyChangedEventArgs.NewValue != null &&
                applicationButton.PreviewDataContext != null)
            {
                applicationButton.ApplyPreviewableContent();
            }
            else
            {
                applicationButton.ReapplyDefaultContent();
            }
        }

        public DataTemplate PreviewTemplate
        {
            get { return (DataTemplate) this.GetValue(PreviewTemplateProperty); }
            set { this.SetValue(PreviewTemplateProperty, value); }
        }

        #endregion

        #region PreviewDataContext

        private static readonly DependencyPropertyKey PreviewDataContextKey = DependencyProperty.RegisterReadOnly(
       "PreviewDataContext", typeof(Object), typeof(ApplicationButton), new PropertyMetadata(default(Object), PreviewDataContextChangedCallback));

        private static readonly DependencyProperty PreviewDataContextProperty = PreviewDataContextKey.DependencyProperty;

        private static void PreviewDataContextChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var applicationButton = (ApplicationButton)dependencyObject;
            if (applicationButton.PreviewEnabled && applicationButton.PreviewTemplate != null &&
                     dependencyPropertyChangedEventArgs.NewValue != null)
            {
                applicationButton.ApplyPreviewableContent();
            }
            else
            {
                applicationButton.ReapplyDefaultContent();
            }
        }

        private Object cachedOriginalContentBeforeExchange;

        public Object PreviewDataContext
        {
            get { return this.GetValue(PreviewDataContextProperty); }
            private set { this.SetValue(PreviewDataContextKey, value); }
        }

        public void Preview([NotNull] Object previewDataContext)
        {
            if (previewDataContext == null)
            {
                throw new ArgumentNullException(nameof(previewDataContext));
            }

            this.PreviewDataContext = previewDataContext;
        }

        public void Unpreview()
        {
            this.PreviewDataContext = null;
        }

        #endregion

        #endregion
        
        private void ApplyPreviewableContent()
        {
            if (!this.IsInitialized)
            {
                return;
            }

            this.cachedOriginalContentBeforeExchange = this.Content;

            var previewableContent = (FrameworkElement)this.PreviewTemplate.LoadContent();
            previewableContent.DataContext = this.PreviewDataContext;

            this.Content = previewableContent;

            this.IsPreviewing = true;
        }

        private void ReapplyDefaultContent()
        {
            if (!this.IsInitialized)
            {
                return;
            }

            if (this.IsPreviewing)
            {
                this.Content = this.cachedOriginalContentBeforeExchange;
            }

            this.cachedOriginalContentBeforeExchange = null;

            this.IsPreviewing = false;
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            var result = base.HitTestCore(hitTestParameters);
            return result ?? new PointHitTestResult(this, hitTestParameters.HitPoint);
        }
    }
}