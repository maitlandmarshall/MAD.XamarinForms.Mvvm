using System;
using System.Runtime.Serialization;

namespace MAD.XamarinForms.Mvvm
{
    [Serializable]
    public class PrepareMethodNotFoundException : Exception
    {
        public PrepareMethodNotFoundException(string message, object[] navigationData) : base(message)
        {
            this.NavigationData = navigationData;
        }

        public object[] NavigationData { get; }
    }
}