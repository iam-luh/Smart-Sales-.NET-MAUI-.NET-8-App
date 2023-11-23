using Smart_Sales.Models;
using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class ProductDisplayPage : ContentPage
{
	private readonly ProductDisplayViewModel viewModel;
	public ProductDisplayPage(ProductDisplayViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.SortAllInvoicesCommand.Execute(new InvoiceList() {
            InvoiceDate=DateTime.Now });
    }

    private void datepicker_DateSelected(object sender, DateChangedEventArgs e)
    {
        viewModel.SortInvoicesonDate(e.NewDate);
    }
}