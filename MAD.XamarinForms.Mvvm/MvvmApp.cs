using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public class MvvmApp
    {
        public static MvvmApp Current { get; set; }

        public IServiceProvider Services { get; internal set; }
        public Application FormsApplication { get; }

        internal MvvmApp(Application formsApplication)
        {
            this.FormsApplication = formsApplication;
        }
    }
}
