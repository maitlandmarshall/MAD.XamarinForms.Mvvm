using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.XamarinForms.Mvvm.Events
{
    public interface IEventProducer
    {
        void Send<TEvent>(TEvent args) where TEvent : IEvent;
    }
}
