namespace JanHafner.Smartbar.Common
{
    using System;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using JetBrains.Annotations;

    public sealed class EventToCommandBehavior : Behavior<FrameworkElement>
    {
        [CanBeNull]
        private Delegate handler;

        [CanBeNull]
        private EventInfo oldEvent;

        [NotNull]
        public static readonly DependencyProperty EventProperty = DependencyProperty.Register("Event", typeof(String), typeof(EventToCommandBehavior), new PropertyMetadata(null, OnEventChanged));

        [NotNull]
        public String Event
        {
            get
            {
                return (String) this.GetValue(EventProperty);
            }
            set
            {
                this.SetValue(EventProperty, value);
            }
        }

        [NotNull]
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommandBehavior), new PropertyMetadata(null));

        [NotNull]
        public ICommand Command
        {
            get
            {
                return (ICommand) this.GetValue(CommandProperty);
            }
            set
            {
                this.SetValue(CommandProperty, value);
            }
        }

        [NotNull]
        public static readonly DependencyProperty PassArgumentsProperty = DependencyProperty.Register("PassArguments", typeof(Boolean), typeof(EventToCommandBehavior), new PropertyMetadata(true));

        public Boolean PassArguments
        {
            get
            {
                return (Boolean) this.GetValue(PassArgumentsProperty);
            }
            set
            {
                this.SetValue(PassArgumentsProperty, value);
            }
        }

        private static void OnEventChanged([NotNull] DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (dependencyObject == null)
            {
                throw new ArgumentNullException(nameof(dependencyObject));
            }

            if (dependencyPropertyChangedEventArgs == null)
            {
                throw new ArgumentNullException(nameof(dependencyPropertyChangedEventArgs));
            }

            var eventToCommandBehavior = (EventToCommandBehavior)dependencyObject;
            if (eventToCommandBehavior.AssociatedObject != null)
            {
                eventToCommandBehavior.AttachHandler((String)dependencyPropertyChangedEventArgs.NewValue);
            }
        }

        protected override void OnAttached()
        {
            this.AttachHandler(this.Event); 
        }

        private void AttachHandler([NotNull] String eventName)
        {
            this.oldEvent?.RemoveEventHandler(this.AssociatedObject, this.handler);

            if (String.IsNullOrWhiteSpace(eventName))
            {
                throw new ArgumentNullException(nameof(eventName));
            }

            var associatedObjectType = this.AssociatedObject.GetType();
            var eventInfo = associatedObjectType.GetEvent(eventName);
            if (eventInfo == null)
            {
                throw new ArgumentException($"The event '{eventName}' was not found on type '{associatedObjectType.Name}'.");
            }
          
            var executeCommandMethodInfo = this.GetType().GetMethod("ExecuteCommand", BindingFlags.Instance | BindingFlags.NonPublic);
            this.handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, executeCommandMethodInfo);
            eventInfo.AddEventHandler(this.AssociatedObject, this.handler);
            this.oldEvent = eventInfo;
        }

        /// <summary>
        /// Executes the Command
        /// </summary>
        [UsedImplicitly]
        private void ExecuteCommand(Object sender, EventArgs eventArgs)
        {
            var parameter = this.PassArguments ? eventArgs : null;
            if (this.Command.CanExecute(parameter))
            {
                this.Command.Execute(parameter);
            }
        }
    }
}