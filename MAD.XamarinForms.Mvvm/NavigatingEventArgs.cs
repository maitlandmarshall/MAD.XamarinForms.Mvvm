using System;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public class NavigatingEventArgs : EventArgs
    {
        internal NavigatingEventArgs(ShellNavigatingEventArgs shellNavigatingEventArgs)
        {
            this.TargetRoute = shellNavigatingEventArgs.Target.Location.ToString();
            this.CurrentRoute = shellNavigatingEventArgs.Current.Location.ToString();
            this.SourceAction = shellNavigatingEventArgs.Source.ToString();
        }

        internal NavigatingEventArgs(string targetRoute)
        {
            TargetRoute = targetRoute;
        }

        public string TargetRoute { get; set; }
        public string SourceAction { get; set; }
        public string CurrentRoute { get; set; }

        public bool Cancel { get; set; } = false;
    }
}