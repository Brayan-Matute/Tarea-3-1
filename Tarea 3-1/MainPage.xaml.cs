using Microsoft.Maui.Controls;
using Tarea_3_1.Views;

namespace Tarea_3_1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCrearProductoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CrearProductoPage());
        }

        private async void OnVerProductosClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProductosPage());
        }

        private void OnSalirClicked(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
