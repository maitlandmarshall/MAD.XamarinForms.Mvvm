using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm.Events
{
    internal class EventService : IEventProducer, IEventConsumer
    {
        private readonly IMessagingCenter messagingCenter;

        public EventService(IMessagingCenter messagingCenter)
        {
            this.messagingCenter = messagingCenter;
        }

        public void Send<TEvent>(TEvent args) where TEvent : IEvent
        {
            this.messagingCenter.Send<EventService, TEvent>(this, typeof(TEvent).FullName, args);
        }

        public void Subscribe<TEvent>(object subscriber, Action<TEvent> callback) where TEvent : IEvent
        {
            this.messagingCenter.Subscribe<EventService, TEvent>(subscriber, typeof(TEvent).FullName, (sender, evt) => callback(evt));
        }

        public void Unsubscribe<TEvent>(object subscriber) where TEvent : IEvent
        {
            this.messagingCenter.Unsubscribe<EventService, TEvent>(subscriber, typeof(TEvent).FullName);
        }
    }
}
