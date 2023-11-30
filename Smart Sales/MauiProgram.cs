using Microsoft.Extensions.Logging;
using Smart_Sales.Services;
using Smart_Sales.ViewModels;
using Smart_Sales.Views;
using Syncfusion.Maui.Core.Hosting;

namespace Smart_Sales
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddSingleton<IInvoiceService, InvoiceService>();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<IPasswordService, PasswordService>();
            builder.Services.AddSingleton<IUsernameService, UsernameService>();


            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddSingleton<InvoicesViewModel>();
            builder.Services.AddTransient<ReportsViewModel>();
            builder.Services.AddTransient<AddInvoiceViewModel>();
            builder.Services.AddSingleton<ProductsViewModel>();
            builder.Services.AddTransient<ProductsDetailsViewModel>();
            builder.Services.AddTransient<ProductDisplayViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
                    
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<ReportsPage>(); 
            builder.Services.AddSingleton<InvoicesPage>();
            builder.Services.AddSingleton<ProductsPage>();
            builder.Services.AddTransient<AddInvoicePage>();
            builder.Services.AddTransient<ProductsDetailsPage>();
            builder.Services.AddTransient<ProductDisplayPage>();
            builder.Services.AddTransient<LoginPage>();


            



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
