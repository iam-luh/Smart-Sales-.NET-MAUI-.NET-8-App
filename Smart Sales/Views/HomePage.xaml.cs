using Smart_Sales.Services;
using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class HomePage : ContentPage
{
	private readonly HomeViewModel viewModel;
    private readonly IUsernameService usernameService;
	public HomePage(HomeViewModel vm, IUsernameService usernameService)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
        this.usernameService = usernameService;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        weekpicker.SelectedIndex = 0;
        viewModel.FilterInvoicesForWeekCommand.Execute(DateTime.Now);
        viewModel.CalculateWeeklyCommand.Execute(null);
        viewModel.GetMostRecentInvoices();
        lblgreet.Text = GetGreeting(DateTime.Now) +", "+ usernameService.GetAllUsernames().Result.LastOrDefault().Name;
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

    public string GetGreeting(DateTime date)
    {
        if (date.Hour >= 0 && date.Hour < 12)
        {
            return "Good morning";
        }
        else if (date.Hour >= 12 && date.Hour < 18)
        {
            return "Good afternoon";
        }
        else
        {
            return "Good evening";
        }

    }


}