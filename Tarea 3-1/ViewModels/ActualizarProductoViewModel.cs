using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Tarea_3_1.Models;
using Tarea_3_1.Services;
using Microsoft.Maui.Controls;

namespace Tarea_3_1.ViewModels
{
    public class ActualizarProductoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService _productoService;
        private ProductoItemViewModel _producto;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action LimpiarImagen;

        public ProductoItemViewModel Producto
        {
            get { return _producto; }
            set
            {
                _producto = value;
                OnPropertyChanged();
            }
        }

        public ICommand ActualizarProductoCommand { get; }

        public ActualizarProductoViewModel(ProductoItemViewModel producto)
        {
            _productoService = new ProductoService();
            Producto = producto;
            ActualizarProductoCommand = new Command(async () => await ActualizarProducto());
        }

        private async Task ActualizarProducto()
        {
            if (Producto == null || string.IsNullOrEmpty(Producto.Id)) return;

            await _productoService.ActualizarProducto(Producto.Id, Producto);

            LimpiarImagen?.Invoke();

            await Application.Current.MainPage.DisplayAlert("Éxito", "Producto actualizado correctamente", "OK");

            MessagingCenter.Send(this, "ProductoActualizado");

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
