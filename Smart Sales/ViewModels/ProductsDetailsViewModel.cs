using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smart_Sales.Models;
using Smart_Sales.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Sales.ViewModels
{
    [QueryProperty(nameof(Myproduct), "Product Detail")]
    public partial class ProductsDetailsViewModel : ObservableObject
    {
        [ObservableProperty]
        public Product myproduct;

        private readonly IProductService _productService;
        public ProductsDetailsViewModel(IProductService productService) 
        {
            Myproduct = new Product();
            _productService = productService;
        }

        [RelayCommand]
        public async Task AddUpdateProduct()
        {
            if (Myproduct.Id > 0)
            {
                Myproduct.LastUpdatedDate= DateTime.Now;
                var response = await _productService.UpdateProduct(Myproduct);
                if(response >  0)
                {
                   await Shell.Current.DisplayAlert("Congratulations!", "The Product has been successfully Updated","OK");
                   await Shell.Current.GoToAsync("..");

                }
                else
                {
                    await Shell.Current.DisplayAlert("Oops!", "Failed To Update the Product", "OK");
                    
                }
            }
            else
            {   Myproduct.CreatedDate = DateTime.Now;
                Myproduct.LastUpdatedDate = DateTime.Now;
                Myproduct.JumlaSoldQty = 0;
                Myproduct.RetailSoldQty= 0;
                Myproduct.TotalQuantity = 0;
                var response = await _productService.AddProduct(Myproduct);
                if (response > 0)
                {
                    await Shell.Current.DisplayAlert("Congratulations!", "The Product has been successfully Added", "OK");
                    await Shell.Current.GoToAsync("..");

                }
                else
                {
                    await Shell.Current.DisplayAlert("Oops!", "Failed To Add the Product", "OK");

                }

            }
        }


    }
}
