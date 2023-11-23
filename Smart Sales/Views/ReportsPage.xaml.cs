using Smart_Sales.Models;
using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class ReportsPage : ContentPage
{
	private readonly ReportsViewModel ViewModel;
	public ReportsPage(ReportsViewModel vm)
	{
		InitializeComponent();
        ViewModel = vm;
        BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        ViewModel.SortallData(new InvoiceList() { InvoiceDate=DateTime.Now }) ;
    }

}