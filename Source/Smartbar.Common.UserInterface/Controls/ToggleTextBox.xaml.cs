namespace JanHafner.Smartbar.Common.UserInterface.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public partial class ToggleTextBox
    {
        public ToggleTextBox()
        {
            this.InitializeComponent();
        }

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof (String), typeof (ToggleTextBox), new PropertyMetadata(default(String)));

        public String Text
        {
            get { return (String) this.GetValue(TextProperty); }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }

        private String oldText;

        #endregion

        #region IsEditing

        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register(
            "IsEditing", typeof (Boolean), typeof (ToggleTextBox), new PropertyMetadata(default(Boolean), IsEditingChangedCallback));

        private static void IsEditingChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var toggleTextBox = (ToggleTextBox)dependencyObject;
            if ((Boolean) dependencyPropertyChangedEventArgs.NewValue)
            {
                toggleTextBox.oldText = toggleTextBox.Text;

                // Seems to be the only way to set the focus to the TextBox right after visibility change.
                toggleTextBox.Dispatcher.InvokeAsync(() =>
                {
                    toggleTextBox.TextBox.Focus();
                    toggleTextBox.TextBox.SelectAll();
                    
                }, DispatcherPriority.ContextIdle);
            }
            else
            {
                toggleTextBox.oldText = null;
            }
        }

        public Boolean IsEditing
        {
            get { return (Boolean) this.GetValue(IsEditingProperty); }
            private set { this.SetValue(IsEditingProperty, value); }
        }

        #endregion

        #region TextBlock Style

        public static readonly DependencyProperty TextBlockStyleOverrideProperty = DependencyProperty.Register(
            "TextBlockStyleOverride", typeof (Style), typeof (ToggleTextBox), new PropertyMetadata(default(Style), TextBlockStyleOverrideChangedCallback));

        private static void TextBlockStyleOverrideChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var toggleTextBox = (ToggleTextBox)dependencyObject;
            toggleTextBox.TextBlock.Style = (Style)dependencyPropertyChangedEventArgs.NewValue;
        }

        public Style TextBlockStyleOverride
        {
            get { return (Style) this.GetValue(TextBlockStyleOverrideProperty); }
            set { this.SetValue(TextBlockStyleOverrideProperty, value); }
        }

        #endregion

        #region IsEditing Tooltip

        public static readonly DependencyProperty ToolTipWhileEditingProperty = DependencyProperty.Register(
            "ToolTipWhileEditing", typeof (String), typeof (ToggleTextBox), new PropertyMetadata(default(String), ToolTipWhileEditingChangedCallback));

        private static void ToolTipWhileEditingChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var toggleTextBox = (ToggleTextBox)dependencyObject;
            toggleTextBox.TextBox.ToolTip = (String)dependencyPropertyChangedEventArgs.NewValue;
        }

        public String ToolTipWhileEditing
        {
            get { return (String) this.GetValue(ToolTipWhileEditingProperty); }
            set { this.SetValue(ToolTipWhileEditingProperty, value); }
        }

        #endregion

        #region EditingEnabled

        public static readonly DependencyProperty EditingEnabledProperty = DependencyProperty.Register(
            "EditingEnabled", typeof (Boolean), typeof (ToggleTextBox), new PropertyMetadata(true));

        public Boolean EditingEnabled
        {
            get { return (Boolean) this.GetValue(EditingEnabledProperty); }
            set { this.SetValue(EditingEnabledProperty, value); }
        }

        #endregion

        #region EditedValueAcceptedCommand

        public static readonly DependencyProperty EditedValueAcceptedCommandProperty = DependencyProperty.Register(
            "EditedValueAcceptedCommand", typeof (ICommand), typeof (ToggleTextBox), new PropertyMetadata(default(ICommand)));

        public ICommand EditedValueAcceptedCommand
        {
            get { return (ICommand) this.GetValue(EditedValueAcceptedCommandProperty); }
            set { this.SetValue(EditedValueAcceptedCommandProperty, value); }
        }

        #endregion

        #region EditedValueDiscardedCommand

        public static readonly DependencyProperty EditedValueDiscardedCommandProperty = DependencyProperty.Register(
            "EditedValueDiscardedCommand", typeof (ICommand), typeof (ToggleTextBox), new PropertyMetadata(default(ICommand)));

        public ICommand EditedValueDiscardedCommand
        {
            get { return (ICommand) this.GetValue(EditedValueDiscardedCommandProperty); }
            set { this.SetValue(EditedValueDiscardedCommandProperty, value); }
        }

        #endregion

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && !this.IsEditing && this.EditingEnabled)
            {
                this.IsEditing = true;

                e.Handled = true;
                return;
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            this.AcceptValue();
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!this.IsEditing || !this.EditingEnabled)
            {
                base.OnPreviewKeyDown(e);
                return;
            }

            switch (e.Key)
            {
                case Key.Enter:
                    this.AcceptValue();

                    e.Handled = true;

                    break;
                case Key.Escape:
                   this.DiscardValue();

                    e.Handled = true;

                    break;
            }
        }

        private void AcceptValue()
        {
            this.IsEditing = false;

            if (this.EditedValueAcceptedCommand != null && this.EditedValueAcceptedCommand.CanExecute(this.Text))
            {
                this.EditedValueAcceptedCommand.Execute(this.Text);
            }
        }

        private void DiscardValue()
        {
            this.Text = this.oldText;
            this.IsEditing = false;

            if (this.EditedValueDiscardedCommand != null && this.EditedValueDiscardedCommand.CanExecute(this.Text))
            {
                this.EditedValueDiscardedCommand.Execute(this.Text);
            }
        }
    }
}
