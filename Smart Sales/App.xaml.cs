using Smart_Sales.Services;
using Smart_Sales.Views;

namespace Smart_Sales
{
    public partial class App : Application
    {
        
        public App(AppShell app, LoginPage loginPage)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH9fcXRdR2FfVkNxXks=");

            InitializeComponent();

            //string key = SecureStorage.GetAsync("password").Result;
            //if(key is null)
            //{
            //    MainPage = loginPage;
            //}

            //else if (key.Equals("LGC19691118"))
            //{
                MainPage = app;
            //}
            //else
            //{
            //    MainPage = loginPage;
            //}

        }
    }
}
