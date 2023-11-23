using Smart_Sales.Models;
using Smart_Sales.ViewModels;
using Syncfusion.Maui.Buttons;


namespace Smart_Sales.Views;

public partial class AddInvoicePage : ContentPage
{
	private readonly AddInvoiceViewModel viewModel;
	public AddInvoicePage(AddInvoiceViewModel vm)
	{
		InitializeComponent();
        viewModel = vm;
        BindingContext = vm;
        

    }


    protected override void OnAppearing()
    {
        var text = viewModel.Myinvoice.Name;
        base.OnAppearing();
        viewModel.GetAllProductsCommand.Execute(null);


        if (viewModel.Myinvoice.IsPaid is true)
        {
            Paid.IsChecked = true;
        }
        else
        { NotPaid.IsChecked = true; }

        for (int i = 0; i < productpicker.ItemsSource.Count; i++)
        {
            var product = productpicker.ItemsSource[i] as Product;
            if (string.Equals(text, product.Name))
            {
                productpicker.SelectedIndex = i;
                break;
            }
        }

        viewModel.Myinvoice.CantEdit = false;
    }


    public void calculateCost()
	{
        var text = entryQty.Text;

        double Qty = double.Parse(text);
		double cost = double.Parse(lblprdcost.Text);

        viewModel.Myinvoice.Cost = Qty * cost;
        lblttlcost.Text = viewModel.Myinvoice.Cost.ToString();

    }

    //category picker method
    private void ctgpicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (viewModel.Myinvoice.CantEdit || productpicker.SelectedItem is null || ctgpicker.SelectedItem is null)
        { return; }

        var picker = sender as Picker;

            var text = picker.SelectedItem as string;

            if (text.Equals("Jumla"))
            {
                viewModel.Myinvoice.ProductCost = viewModel.Myproduct.JumlaPrice;
                lblprdcost.Text = viewModel.Myproduct.JumlaPrice.ToString();

                calculateCost();
            }
            else
            {
                viewModel.Myinvoice.ProductCost = viewModel.Myproduct.RetailPrice;
                lblprdcost.Text = viewModel.Myproduct.RetailPrice.ToString();

                calculateCost();
            }
        
    }

    //entry for the quantity method
    private void entryQty_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (viewModel.Myinvoice.CantEdit )
        { return; }
        var text = e.NewTextValue;
        if (string.IsNullOrWhiteSpace(text)) {  return; }

        double Qty = double.Parse(text);

        viewModel.Myinvoice.Cost = Qty * viewModel.Myinvoice.ProductCost;
        lblttlcost.Text = viewModel.Myinvoice.Cost.ToString();
    }

    // method for the picker of products
    private void productpicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(viewModel.Myinvoice.CantEdit || productpicker.SelectedItem is null || ctgpicker.SelectedItem is null)
        { return; }
        
        var text = ctgpicker.SelectedItem as string;
        if (text.Equals("Jumla"))
        {
            viewModel.Myinvoice.ProductCost = viewModel.Myproduct.JumlaPrice;
            lblprdcost.Text = viewModel.Myproduct.JumlaPrice.ToString();

            if (string.IsNullOrWhiteSpace(text)) { return; }
            calculateCost();

        }
        else if (text.Equals("Retail"))
        {
            viewModel.Myinvoice.ProductCost = viewModel.Myproduct.RetailPrice;
            lblprdcost.Text = viewModel.Myproduct.RetailPrice.ToString();

            if (string.IsNullOrWhiteSpace(text)) { return; }
            calculateCost();


        }
    }

    // method for the entry which displays the product cost
    private void lblprdcost_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(viewModel.Myinvoice.CantEdit) 
        { return; }
        var text = entryQty.Text;

        if (string.IsNullOrWhiteSpace(text)) { return; }
        double Qty = double.Parse(text);

        viewModel.Myinvoice.Cost = Qty * viewModel.Myinvoice.ProductCost;
        lblttlcost.Text = viewModel.Myinvoice.Cost.ToString();
    }

    // method for the radio button which picks paid or not paid
    

    private void ExpensebtnGrp_CheckedChanged_1(object sender, Syncfusion.Maui.Buttons.CheckedChangedEventArgs e)
    {
        var btn = e.CurrentItem;

        var text = btn.Text;
        if (text.Equals("Paid"))
        {
            viewModel.Myinvoice.IsPaid = true;
            viewModel.Myinvoice.IsNotPaid = false;

        }
        else
        {
            viewModel.Myinvoice.IsPaid = false;
            viewModel.Myinvoice.IsNotPaid = true;

        }
    }
}