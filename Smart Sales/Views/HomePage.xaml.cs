using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel viewModel;
	public HomePage(HomeViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        weekpicker.SelectedIndex = 0;
        viewModel.FilterInvoicesForWeekCommand.Execute(DateTime.Now);
        viewModel.CalculateWeeklyCommand.Execute(null);
        viewModel.GetMostRecentInvoices();
    }

    private void weekpicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        
        int index = picker.SelectedIndex;

        if(index == 0)
        {
            var date = DateTime.Now;
            viewModel.FilterInvoicesForWeekCommand.Execute(date);
        }
        else if(index == 1)
        {
            var date = DateTime.Now.AddDays(-7);
            viewModel.FilterInvoicesForWeekCommand.Execute(date);

        }
        else if(index == 2)
        {
            var date = DateTime.Now.AddDays(-14);
            viewModel.FilterInvoicesForWeekCommand.Execute(date);
        }


    }
}