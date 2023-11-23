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

    
}