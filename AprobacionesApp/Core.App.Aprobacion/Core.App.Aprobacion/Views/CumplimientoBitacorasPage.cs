using System;

using Xamarin.Forms;

namespace Core.App.Aprobacion.Views
{
    public class CumplimientoBitacorasPage : ContentPage
    {
        public CumplimientoBitacorasPage()
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

