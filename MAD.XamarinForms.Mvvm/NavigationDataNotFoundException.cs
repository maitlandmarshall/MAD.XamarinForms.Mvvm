using System;
using System.Runtime.Serialization;

namespace MAD.XamarinForms.Mvvm
{
    [Serializable]
    internal class NavigationDataNotFoundException : Exception
    {
        public NavigationDataNotFoundException()
        {
        }

        public NavigationDataNotFoundException(string message) : base(message)
        {
        }

        public NavigationDataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NavigationDataNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}