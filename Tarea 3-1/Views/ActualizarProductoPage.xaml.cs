using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using Tarea_3_1.ViewModels;
using Microsoft.Maui.Controls;

namespace Tarea_3_1.Views
{
    public partial class ActualizarProductoPage : ContentPage
    {
        public ActualizarProductoPage(ProductoItemViewModel producto)
        {
            InitializeComponent();
            var viewModel = new ActualizarProductoViewModel(producto);
            BindingContext = viewModel;
            viewModel.LimpiarImagen += OnLimpiarImagen;
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = $"{DateTime.UtcNow}.jpg"
            });

            if (file == null)
                return;

            if (BindingContext is ActualizarProductoViewModel viewModel)
            {
                viewModel.Producto.Foto = file.Path;
                imagen.Source = ImageSource.FromFile(file.Path);
            }
        }

        private void OnLimpiarImagen()
        {
            imagen.Source = "smiley.png";
        }
    }
}
