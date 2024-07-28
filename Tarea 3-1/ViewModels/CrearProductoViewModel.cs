using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Tarea_3_1.Models;
using System.Threading.Tasks;
using Tarea_3_1.Services;
using System.Diagnostics;

namespace Tarea_3_1.ViewModels
{
    public class CrearProductoViewModel : INotifyPropertyChanged
    {
        private readonly ProductoService _productoService;
        private Producto _producto;

        public event PropertyChangedEventHandler PropertyChanged;
        public event Action LimpiarImagen;

        public Producto Producto
        {
            get { return _producto; }
            set
            {
                _producto = value;
                OnPropertyChanged(nameof(Producto));
            }
        }

        public ICommand CrearProductoCommand { get; }

        public CrearProductoViewModel()
        {
            _productoService = new ProductoService();
            Producto = new Producto();
            CrearProductoCommand = new Command(async () => await CrearProducto());
        }

        private async Task CrearProducto()
        {
            Debug.WriteLine("CrearProducto: Inicio del método.");
            try
            {
                if (Producto == null)
                {
                    Debug.WriteLine("CrearProducto: Producto es nulo.");
                    throw new ArgumentNullException(nameof(Producto));
                }

                if (string.IsNullOrWhiteSpace(Producto.Nombre) ||
                    string.IsNullOrWhiteSpace(Producto.Descripción) ||
                    Producto.Precio == null ||
                    string.IsNullOrWhiteSpace(Producto.Foto))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
                    return;
                }

                Debug.WriteLine("CrearProducto: Producto no es nulo y todos los campos están completos.");
                await _productoService.CrearProducto(Producto);

                await Application.Current.MainPage.DisplayAlert("Éxito", "Producto creado exitosamente.", "OK");

            
                Producto = new Producto();
                OnPropertyChanged(nameof(Producto));

                
                LimpiarImagen?.Invoke();
                
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
             /*   Debug.WriteLine($"Error al crear producto: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", "Ocurrió un error al crear el producto.", "OK");*/
            }
            Debug.WriteLine("CrearProducto: Fin del método.");
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
