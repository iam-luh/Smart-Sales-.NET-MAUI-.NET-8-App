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
        //ViewModel.SortallData(new InvoiceList() { InvoiceDate=DateTime.Now });
        if (ViewModel.Timeperiod.Equals("Years"))
        {
            ViewModel.SortallDataCommand.Execute(new InvoiceList() { InvoiceDate = DateTime.Now.AddYears((int)(tabView.SelectedIndex - 11)) });
        }
        else
        {  
            ViewModel.SortallDataMonthsCommand.Execute(new InvoiceList() { InvoiceDate = DateTime.Now.AddMonths((int)(tabView.SelectedIndex - 11)) });
        }


    }

    private async void Imgbtncalendar_Clicked(object sender, EventArgs e)
    {
        await ViewModel.SelectTimePeriod();
        lblcalendar.Text = ViewModel.Timeperiod;
        if(ViewModel.Timeperiod.Equals("Years"))
        {
            ViewModel.CreateDateCollection();
            ViewModel.SortallDataCommand.Execute(new InvoiceList() { InvoiceDate = DateTime.Now.AddYears((int)(tabView.SelectedIndex - 11))});
        }
        else
        {
            ViewModel.CreateMonthsDateCollection();
            ViewModel.SortallDataMonthsCommand.Execute(new InvoiceList() { InvoiceDate = DateTime.Now.AddMonths((int)(tabView.SelectedIndex - 11))});
        }

    }
}