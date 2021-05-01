using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MAD.XamarinForms.Mvvm
{
    public class ViewModelLifecycleBehavior : Behavior<VisualElement>
    {
        private readonly IServiceScope serviceScope;
        private readonly ViewModel viewModel;

        private VisualElement associatedObject;
        private bool initialized = false;

        public ViewModelLifecycleBehavior(IServiceScope serviceScope, ViewModel viewModel)
        {
            this.serviceScope = serviceScope;
            this.viewModel = viewModel;
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            
            this.associatedObject = bindable;
            this.AddEventHandlers();
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            base.OnDetachingFrom(bindable);
            this.Dispose();
        }

        private void AddEventHandlers()
        {
            this.associatedObject.PropertyChanged += this.VisualElement_PropertyChanged;

            if (this.associatedObject is ContentPage cp)
            {
                cp.Appearing += ContentPage_Appearing;
                cp.Disappearing += ContentPage_Disappearing;
            }
        }

        private void RemoveEventHandlers()
        {
            this.associatedObject.PropertyChanged -= this.VisualElement_PropertyChanged;

            if (this.associatedObject is ContentPage cp)
            {
                cp.Appearing -= ContentPage_Appearing;
                cp.Disappearing -= ContentPage_Disappearing;
            }
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            this.viewModel.ViewDisappearing();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            this.viewModel.ViewAppearing();
        }

        private void VisualElement_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Content":
                    this.OnContentChanged();
                    break;
                case nameof(VisualElement.Parent):
                    this.OnParentChanged();
                    break;
            }
        }

        private async void OnContentChanged()
        {
            if (this.initialized)
                return;

            try
            {
                if (this.associatedObject is ContentView contentView)
                {
                    // A ContentView will not be the root layout in the layout tree, unlike a ContentPage
                    // In order to allow BindableProperty bindings from a ContentPage -> ContentView to function
                    // while also allowing the ContentView to have a functional ViewModel, we must set the content view's Content binding context instead.
                    contentView.Content.BindingContext = this.viewModel;
                }
                else
                {
                    this.associatedObject.BindingContext = this.viewModel;
                }

                await this.viewModel.Initialize();
            }
            finally
            {
                this.initialized = true;
            }
        }

        private void OnParentChanged()
        {
            if (this.associatedObject.Parent is null)
            {
                this.Dispose();
            }
        }

        private void Dispose()
        {
            try
            {
                this.viewModel.ViewDestroy();
            }
            finally
            {
                this.serviceScope.Dispose();
                this.RemoveEventHandlers();
            }
        }
    }
}
