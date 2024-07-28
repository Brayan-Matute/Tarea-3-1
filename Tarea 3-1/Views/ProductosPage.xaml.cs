using Tarea_3_1.ViewModels;

namespace Tarea_3_1.Views;

public partial class ProductosPage : ContentPage
{
    private readonly ProductoViewModel _viewModel;

    public ProductosPage()
    {
        InitializeComponent();
        _viewModel = new ProductoViewModel();
        BindingContext = _viewModel;
    }
}

