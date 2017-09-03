namespace JanHafner.Smartbar.Common.UserInterface.Controls
{
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Markup;

    /// <summary>
    /// Interaction logic for DialogControlBar.xaml
    /// </summary>
    [ContentProperty("Controls")]
    public partial class DialogControlBar
    {
        public DialogControlBar()
        {
            this.InitializeComponent();

            this.Controls = new Collection<UIElement>();
        }

        public static readonly DependencyProperty ControlsProperty = DependencyProperty.Register(
            "Controls", typeof (Collection<UIElement>), typeof (DialogControlBar), new PropertyMetadata(new Collection<UIElement>()));

        public Collection<UIElement> Controls
        {
            get { return (Collection<UIElement>) this.GetValue(ControlsProperty); }
            set { this.SetValue(ControlsProperty, value); }
        }
    }
}
