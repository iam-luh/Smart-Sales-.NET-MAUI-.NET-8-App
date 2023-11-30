using Smart_Sales.Models;
using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class InvoicesPage : ContentPage
{
	private readonly InvoicesViewModel viewModel;
	public InvoicesPage(InvoicesViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
        
		
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.SortAllInvoicesCommand.Execute(new InvoiceList() { InvoiceDate=DateTime.Now.AddMonths((int)(tabView.SelectedIndex - 11))});
    }


}