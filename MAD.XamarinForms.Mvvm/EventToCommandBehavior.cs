using System;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public class EventToCommandBehavior : Behavior<VisualElement>
    {
        private Delegate eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior), null);
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior), null);

        public string EventName
        {
            get => (string)this.GetValue(EventNameProperty);
            set => this.SetValue(EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter)this.GetValue(InputConverterProperty);
            set => this.SetValue(InputConverterProperty, value);
        }

        public VisualElement AssociatedObject { get; private set; }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);

            this.AssociatedObject = bindable;
            this.AssociatedObject.BindingContextChanged += this.AssociatedObject_BindingContextChanged;

            this.RegisterEvent(this.EventName);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            base.OnDetachingFrom(bindable);

            this.AssociatedObject.BindingContextChanged -= this.AssociatedObject_BindingContextChanged;

            this.DeregisterEvent(this.EventName);
        }

        private void AssociatedObject_BindingContextChanged(object sender, EventArgs e)
        {
            this.BindingContext = this.AssociatedObject.BindingContext;
        }

        private void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            EventInfo eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);

            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't register the '{0}' event.", this.EventName));
            }

            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            this.eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(this.AssociatedObject, this.eventHandler);
        }

        private void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (this.eventHandler == null)
            {
                return;
            }

            EventInfo eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", this.EventName));
            }

            eventInfo.RemoveEventHandler(this.AssociatedObject, this.eventHandler);
            this.eventHandler = null;
        }

        private void OnEvent(object sender, object eventArgs)
        {
            if (this.Command == null)
            {
                return;
            }

            object resolvedParameter;
            var eventToCommandArgs = this.CreateEventToCommandArgs();

            if (eventToCommandArgs != null)
            {
                eventToCommandArgs.SetCommandParameter(this.CommandParameter);
                eventToCommandArgs.SetEventArgs(eventArgs as EventArgs);

                resolvedParameter = eventToCommandArgs;
            }
            else
            {
                if (this.CommandParameter != null)
                {
                    resolvedParameter = this.CommandParameter;
                }
                else if (this.Converter != null)
                {
                    resolvedParameter = this.Converter.Convert(eventArgs, typeof(object), null, null);
                }
                else
                {
                    resolvedParameter = eventArgs;
                }
            }

            if (this.Command.CanExecute(resolvedParameter))
            {
                this.Command.Execute(resolvedParameter);
            }
        }

        private IEventToCommandArgs CreateEventToCommandArgs()
        {
            var commandGenericArguments = this.Command.GetType().GetGenericArguments();

            if (commandGenericArguments.Length == 0)
                return null;

            var genericArgument = commandGenericArguments.FirstOrDefault();

            if (typeof(IEventToCommandArgs).IsAssignableFrom(genericArgument))
            {
                return Activator.CreateInstance(genericArgument) as IEventToCommandArgs;
            }
            else
            {
                return null;
            }
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}
