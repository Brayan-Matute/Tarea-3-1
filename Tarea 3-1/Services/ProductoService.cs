using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tarea_3_1.Models;

namespace Tarea_3_1.Services
{
    public class ProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://tarea-3-1-c69c0-default-rtdb.firebaseio.com/");
           /* _httpClient.BaseAddress = new Uri("https://pm2-g4-default-rtdb.firebaseio.com/");*/
        }

        public async Task<List<Producto>> ObtenerProductos()
        {
            try
            {
                var response = await _httpClient.GetStringAsync("productos.json");
                var productos = JsonConvert.DeserializeObject<Dictionary<string, Producto>>(response);

                if (productos == null)
                {
                    return new List<Producto>();
                }

                foreach (var kvp in productos)
                {
                    kvp.Value.Id = kvp.Key;
                }

                return productos.Values.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener productos: {ex.Message}");
                return new List<Producto>();
            }
        }


        public async Task CrearProducto(Producto producto)
        {
            if (producto == null)
            {
                Debug.WriteLine("CrearProducto en ProductoService: Producto es nulo.");
                throw new ArgumentNullException(nameof(producto));
            }

            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Debug.WriteLine("CrearProducto en ProductoService: Antes de enviar la solicitud POST.");

            var response = await _httpClient.PostAsync("productos.json", content);

            if (!response.IsSuccessStatusCode)
            {
               
            }

            Debug.WriteLine("CrearProducto en ProductoService: Producto creado exitosamente.");
        }


        public async Task ActualizarProducto(string id, object producto)
        {
            var json = JsonConvert.SerializeObject(producto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await _httpClient.PutAsync($"productos/{id}.json", content);
        }

        public async Task EliminarProducto(string id)
        {
            await _httpClient.DeleteAsync($"productos/{id}.json");
        }
    }

}