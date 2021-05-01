using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.XamarinForms.Mvvm.Events
{
    public interface IEventConsumer
    {
        void Subscribe<TEvent>(object subscriber, Action<TEvent> callback) where TEvent : IEvent;
        void Unsubscribe<TEvent>(object subscriber) where TEvent : IEvent;
    }
}
