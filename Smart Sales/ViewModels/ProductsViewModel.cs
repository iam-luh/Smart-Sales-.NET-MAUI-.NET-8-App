using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smart_Sales.Models;
using Smart_Sales.Services;
using Smart_Sales.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Sales.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        public ProductsViewModel(IProductService productService)
        {
            _productService = productService;
        }

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        [RelayCommand]
        public async Task GetAllProducts()
        {
            var productlist = await _productService.GetAllProducts();
            if (productlist?.Count>=0)
            {
                Products.Clear();
                foreach (var product in productlist) 
                {
                  Products.Add(product);
                }
            }
        }

        [RelayCommand]
        public async Task DeleteProduct(Product product) 
        { 
            bool ans =  await Shell.Current.DisplayAlert("DELETE PRODUCT?", "Would you like to delete this product?", "Yes", "No");
            if(ans) 
            {
                var delresponse = await _productService.DeleteProduct(product);
                if (delresponse > 0)
                {
                    await GetAllProducts();
                }
            }
            else
            {
                return;
            }

            
        }

        [RelayCommand] 
        public async Task GoToProductDisplay(Product product)
        {
            var taskdict = new Dictionary<string, object>();
            taskdict.Add("Product Detail", product);
            await Shell.Current.GoToAsync(nameof(ProductDisplayPage), true, taskdict);
        }

        [RelayCommand]
        public async Task GoToProductDetails(Product product)
        {
            var taskdict = new Dictionary<string, object>();
            taskdict.Add("Product Detail", product);
            await Shell.Current.GoToAsync(nameof(ProductsDetailsPage), true, taskdict);
        }

        [RelayCommand]
        public async Task GoToProductDtls()
        {
            
            await Shell.Current.GoToAsync(nameof(ProductsDetailsPage));
        }

    }
}
