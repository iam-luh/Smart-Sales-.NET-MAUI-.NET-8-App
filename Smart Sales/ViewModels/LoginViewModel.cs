using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smart_Sales.Models;
using Smart_Sales.Services;
using Smart_Sales.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Sales.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
       private readonly IPasswordService passwordService;
       private readonly IUsernameService usernameService;
        public LoginViewModel(IPasswordService passwordService, IUsernameService usernameService)  
        {
           this.passwordService = passwordService;
           this.usernameService = usernameService;
        }

        
        public async void checklogin(string myname, string key)
        {
            if (string.IsNullOrWhiteSpace(myname) || string.IsNullOrWhiteSpace(key)) 
            {
                return;
            }

            if (key.Equals("LGC19691118"))
            {
                _ = passwordService.AddPassword(new Password() { CreatedDate = DateTime.Now, LicenseTime = new TimeSpan(36500, 0, 0, 0), Name = key });
                _ = usernameService.AddUsername(new Username() { CreatedDate = DateTime.Now, Name = myname });
                return;
            }
            var allkeys = passwordService.GetAllPasswords().Result;

            foreach(var pass in allkeys)
            {
                if(pass.Name.Equals(key)) 
                { 
                    return;
                }
            }

            int result; bool for6months = false; bool for12months = false;
            try
            {
                // Convert the string to an integer using int.Parse
                 result = int.Parse($"{key.ElementAt(1)}"+ $"{key.ElementAt(3)}" + $"{key.ElementAt(5)}" + $"{key.ElementAt(7)}");
                 for6months = (int.Parse($"{key.ElementAt(1)}") + int.Parse($"{key.ElementAt(3)}") + int.Parse($"{key.ElementAt(5)}") + int.Parse($"{key.ElementAt(7)}")) == 21;
                 for12months = (int.Parse($"{key.ElementAt(1)}") + int.Parse($"{key.ElementAt(3)}") + int.Parse($"{key.ElementAt(5)}") + int.Parse($"{key.ElementAt(7)}")) == 27;

                // Display the result
            }
            catch (Exception )
            {
                return;
            }


            if ( result%3 is 0 && for6months)
            {
                _ = passwordService.AddPassword(new Password() { CreatedDate = DateTime.Now, LicenseTime= new TimeSpan(0,0,3,0), Name=key }) ;
                _ = usernameService.AddUsername(new Username() { CreatedDate = DateTime.Now, Name = myname });

                return;
            }
            if (result % 3 is 0 && for12months)
            {
                _ = passwordService.AddPassword(new Password() { CreatedDate = DateTime.Now, LicenseTime = new TimeSpan(0, 0, 5, 0), Name = key });
                _ = usernameService.AddUsername(new Username() { CreatedDate = DateTime.Now, Name = myname });
                return;
            }

        }

    }
}
