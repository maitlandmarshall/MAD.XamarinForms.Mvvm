using System;
using System.Collections.Generic;
using System.Text;

namespace MAD.XamarinForms.Mvvm
{
    internal interface IEventToCommandArgs
    {
        void SetEventArgs(EventArgs eventArgs);
        void SetCommandParameter(object commandParameter);
    }

    public class EventToCommandArgs<TEventArgs, TCommandParameter> : IEventToCommandArgs
        where TEventArgs : EventArgs
    {
        public TEventArgs EventArgs { get; internal set; }
        public TCommandParameter CommandParameter { get; internal set; }

        void IEventToCommandArgs.SetCommandParameter(object commandParameter)
        {
            this.CommandParameter = (TCommandParameter)commandParameter;
        }

        void IEventToCommandArgs.SetEventArgs(EventArgs eventArgs)
        {
            this.EventArgs = eventArgs as TEventArgs;
        }
    }
}
