using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
       
        public LoginViewModel() 
        {
           
        }

        
        public async void checklogin(string name, string key)
        {
            if (string.IsNullOrWhiteSpace(name)) 
            {
                return;
            }

            //if (key.Equals("LGC19691118"))
            //{
            //    await SecureStorage.SetAsync("password", key);
            //    await SecureStorage.SetAsync("username", name);         
                
            //}
            else
            {
                return;
            }

        }

    }
}
