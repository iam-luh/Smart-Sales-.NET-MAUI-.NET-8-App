using Smart_Sales.Services;
using Smart_Sales.Views;

namespace Smart_Sales
{
    public partial class App : Application
    {
        
        public App(AppShell app, LoginPage loginPage, IPasswordService passwordService)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH9fcXRdR2FfVkNxXks=");

            InitializeComponent();

            //string key = SecureStorage.Default.GetAsync("password").Result;
            
            var keys = passwordService.GetAllPasswords().Result;
            var key = keys.LastOrDefault();

            if (keys.Count is 0)
            {
                MainPage = loginPage;
                return;
            }

            if (key.Name == "LGC19691118")
            {
                MainPage = app;
                return;
            }

            


            var diff = DateTime.Now - key.CreatedDate;

            bool expired = false;
            if (diff.Minutes > key.LicenseTime.Minutes)
            {
                expired = true;
            }

            if (diff.Days > key.LicenseTime.Days) 
            {
                if(diff.Minutes > key.LicenseTime.Minutes)
                {
                    expired = true;
                }
                 
            }
            

            MainPage = expired ? loginPage : app;

        }
    }
}
