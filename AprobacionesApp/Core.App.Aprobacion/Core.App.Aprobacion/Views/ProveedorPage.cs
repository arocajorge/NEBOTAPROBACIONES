using System;

using Xamarin.Forms;

namespace Core.App.Aprobacion.Views
{
    public class ProveedorPage : ContentPage
    {
        public ProveedorPage()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

