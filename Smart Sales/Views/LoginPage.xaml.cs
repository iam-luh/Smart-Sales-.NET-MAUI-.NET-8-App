using Smart_Sales.ViewModels;

namespace Smart_Sales.Views;

public partial class LoginPage : ContentPage
{
	private readonly LoginViewModel viewModel;
	public LoginPage(LoginViewModel viewModel)
	{
		InitializeComponent();
        this.viewModel = viewModel;
        BindingContext = viewModel;
	}

    private void btnlogin_Clicked(object sender, EventArgs e)
    {
        var name = entryname.Text;
        var key = entrykey.Text;
        viewModel.checklogin(name, key);
    }
}