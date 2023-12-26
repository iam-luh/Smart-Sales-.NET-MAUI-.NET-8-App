using Smart_Sales.Views;

namespace Smart_Sales
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(HomePage),typeof(HomePage));
            Routing.RegisterRoute(nameof(InvoicesPage),typeof(InvoicesPage));
            Routing.RegisterRoute(nameof(AddInvoicePage),typeof(AddInvoicePage));
            Routing.RegisterRoute(nameof(ProductsPage),typeof(ProductsPage));
            Routing.RegisterRoute(nameof(ReportsPage),typeof(ReportsPage));
            Routing.RegisterRoute(nameof(ProductsDetailsPage),typeof(ProductsDetailsPage));
            Routing.RegisterRoute(nameof(ProductDisplayPage),typeof(ProductDisplayPage));
            Routing.RegisterRoute(nameof(LoginPage),typeof(LoginPage));
            Routing.RegisterRoute(nameof(ProductHistoryPage),typeof(ProductHistoryPage));
        }
    }
}
