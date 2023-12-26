using Smart_Sales.Models;
using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class ProductHistoryPage : ContentPage
{
	private readonly ProductHistoryViewModel viewModel;
	public ProductHistoryPage(ProductHistoryViewModel viewModel)
	{
		InitializeComponent();
		this.viewModel = viewModel;
		BindingContext = viewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		viewModel.SortAllInvoicesCommand.Execute(new InvoiceList(){ InvoiceDate=DateTime.Now});
    }
}