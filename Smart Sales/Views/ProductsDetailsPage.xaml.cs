using Smart_Sales.ViewModels;
using Syncfusion.Maui.Buttons;

namespace Smart_Sales.Views;

public partial class ProductsDetailsPage : ContentPage
{
	private readonly ProductsDetailsViewModel viewModel;
	public ProductsDetailsPage(ProductsDetailsViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
	}

    private void unitpicker_SelectedIndexChanged(object sender, EventArgs e)
    {
		var picker = sender as Picker;
		viewModel.Myproduct.ProductUnit = picker.SelectedItem as string;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if (viewModel.Myproduct.IsExpense is true)
        {
            expense.IsChecked = true;
		}
		else if(viewModel.Myproduct.IsExpense is false)
		{ income.IsChecked = true; }			
    }

    private void ExpensebtnGrp_CheckedChanged(object sender, Syncfusion.Maui.Buttons.CheckedChangedEventArgs e)
    {
		var btn = e.CurrentItem;		
	
		var text = btn.Text;
		if (text.Equals("Expense"))
		{
			viewModel.Myproduct.IsExpense = true;
		}
		else
		{
			viewModel.Myproduct.IsExpense= false;
		}
    }
}