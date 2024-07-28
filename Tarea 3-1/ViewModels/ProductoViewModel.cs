using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Tarea_3_1.Models;
using Tarea_3_1.Services;
using Microsoft.Maui.Controls;

namespace Tarea_3_1.ViewModels
{
    public class ProductoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService _productoService;
        private ObservableCollection<ProductoItemViewModel> _productos;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ProductoItemViewModel> Productos
        {
            get { return _productos; }
            set
            {
                _productos = value ?? new ObservableCollection<ProductoItemViewModel>();
                OnPropertyChanged();
            }
        }

        public ICommand EliminarProductoCommand { get; }
        public ICommand ActualizarProductoCommand { get; }

        public ProductoViewModel()
        {
            _productoService = new ProductoService();
            Productos = new ObservableCollection<ProductoItemViewModel>();
            EliminarProductoCommand = new Command<ProductoItemViewModel>(async (producto) => await EliminarProducto(producto));
            ActualizarProductoCommand = new Command<ProductoItemViewModel>(async (producto) => await ActualizarProducto(producto));
            CargarProductos();

            MessagingCenter.Subscribe<ActualizarProductoViewModel>(this, "ProductoActualizado", async (sender) => await CargarProductos());
        }

        private async Task CargarProductos()
        {
            try
            {
                var productos = await _productoService.ObtenerProductos();
                var productosViewModel = productos.Select(p => new ProductoItemViewModel(p)).ToList();
                Productos = new ObservableCollection<ProductoItemViewModel>(productosViewModel ?? new List<ProductoItemViewModel>());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al cargar productos: {ex.Message}");
                Productos = new ObservableCollection<ProductoItemViewModel>();
            }
        }

        private async Task EliminarProducto(ProductoItemViewModel producto)
        {
            if (producto == null || string.IsNullOrEmpty(producto.Id)) return;

            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Desea eliminar este producto?", "Sí", "No");
            if (confirm)
            {
                await _productoService.EliminarProducto(producto.Id);
                Productos.Remove(producto);
            }
        }

        private async Task ActualizarProducto(ProductoItemViewModel producto)
        {
            if (producto == null || string.IsNullOrEmpty(producto.Id)) return;

            await Application.Current.MainPage.Navigation.PushAsync(new Views.ActualizarProductoPage(producto));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ProductoItemViewModel : Producto
    {
        public ProductoItemViewModel(Producto producto)
        {
            Id = producto.Id;
            Nombre = producto.Nombre;
            Descripción = producto.Descripción;
            Precio = producto.Precio;
            Foto = producto.Foto;
        }

        public ImageSource ImageSource
        {
            get
            {
                if (string.IsNullOrEmpty(Foto))
                {
                    return null;
                }
                try
                {
                    return ImageSource.FromFile(Foto);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error al cargar la imagen desde la ruta: {ex.Message}");
                    return null;
                }
            }
        }
    }
}
