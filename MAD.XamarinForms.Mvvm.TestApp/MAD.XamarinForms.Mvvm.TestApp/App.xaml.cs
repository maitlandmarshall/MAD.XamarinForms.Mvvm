using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MAD.XamarinForms.Mvvm.TestApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Mvvm.Init<Startup>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
