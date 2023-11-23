using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class ProductsPage : ContentPage
{
	private readonly ProductsViewModel viewModel;
	public ProductsPage(ProductsViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.GetAllProductsCommand.Execute(null);
    }
}