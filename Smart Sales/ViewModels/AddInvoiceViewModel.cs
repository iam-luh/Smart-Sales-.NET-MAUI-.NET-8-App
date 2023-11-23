using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smart_Sales.Models;
using Smart_Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Sales.ViewModels
{
    [QueryProperty(nameof(Myinvoice),"Invoice Detail")]
    public partial class AddInvoiceViewModel : ObservableObject
    {
        public ObservableCollection<Product> Allproducts { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<String> Allproductnames { get; set; } = new ObservableCollection<String>();

        [ObservableProperty]
        public Invoice myinvoice;

        [ObservableProperty]
        public Product myproduct;

        [ObservableProperty]
        public TimeSpan invoiceTime;

        private readonly IProductService productService;
        private readonly IInvoiceService invoiceService;
        public AddInvoiceViewModel(IProductService productService,IInvoiceService invoiceService)
        {
            Myinvoice = new Invoice()
            {
                Quantity = 0,
                CreatedDate = DateTime.Now,
                CreatedTime = DateTime.Now.TimeOfDay,
                IsPaid = false,
                IsNotPaid = true,
                CantEdit = false
            };

            Myproduct = new Product()
            {
               
            };
            
            this.productService = productService;
            this.invoiceService = invoiceService;
        }

        [RelayCommand]
        public  void GetAllProducts()
        {
            var productlist = productService.GetAllProducts().Result ;

            Allproducts.Clear();


            foreach (var product in productlist)
            {
                Allproducts.Add(product);
            }

        }

        [RelayCommand]
        public void GetProductNames()
        {           
            var productlist =  Allproducts;
            Allproductnames.Clear();
            foreach(var product in productlist)
            {
                Allproductnames.Add(product.Name);
            }

        }

        [RelayCommand]
        public void CreateInvoice()
        {
            Myinvoice = new Invoice()
            {
                Quantity = 0,
                CreatedDate = DateTime.Now,
                IsNotPaid = true,
                IsPaid= false,
                CreatedTime = DateTime.Now.TimeOfDay,
                CantEdit = false
                
            };
        }

        [RelayCommand]
        public void CalculateCost(double num)
        {   
            Myinvoice.Cost = num * Myinvoice.ProductCost;

        }

        [RelayCommand]
        public async Task AddUpdateInvoice()
        {   if(Myinvoice is null)
            {
                await Shell.Current.DisplayAlert("NO INVOICE!", "An Invoice hasn't been created", "OK");
                return;
            }
            
            if (Myproduct is null)
            {
                await Shell.Current.DisplayAlert("N0 PRODUCT PICKED!", "You haven't choosed a Product", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Myinvoice.CustomerName)) 
            {
                await Shell.Current.DisplayAlert("NO CUSTOMER NAME!", "You haven't entered a Customer Name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Myinvoice.Category))
            {
                await Shell.Current.DisplayAlert("NO CATEGORY CHOOSEN!", "You haven't choosed a Category", "OK");
                return;
            }

            if (Myinvoice.Quantity <= 0)
            {
                await Shell.Current.DisplayAlert("NO QUANTITY ENTERED!", "You haven't entered a Product Quantity", "OK");
                return;
            }

            if (Myinvoice.Id > 0)
            {              
                Myinvoice.Name = Myproduct.Name;
                Myinvoice.IsExpense = Myproduct.IsExpense;
                Myinvoice.ProductUnit = Myproduct.ProductUnit;
                Myinvoice.LastUpdatedDate = Myinvoice.CreatedDate.Date + DateTime.Today.Add(Myinvoice.CreatedTime).TimeOfDay;
                // Changing the product quantity with the previous invoice
                var invoice =  invoiceService.GetAllInvoices().Result.ElementAt
                    (invoiceService.GetAllInvoices().Result.FindIndex(i => i.Id == Myinvoice.Id));

                Myproduct.TotalQuantity -= invoice.Quantity;
                if (Myinvoice.Category.Equals("Jumla"))
                {
                    Myproduct.JumlaSoldQty -= invoice.Quantity;
                }
                else
                {
                    Myproduct.RetailSoldQty -= invoice.Quantity;
                }
                _ = productService.UpdateProduct(Myproduct).Result;

                var response = await invoiceService.UpdateInvoice(Myinvoice);

                // Changing the product quantity with the new invoice
                Myproduct.TotalQuantity += Myinvoice.Quantity;
                if (Myinvoice.Category.Equals("Jumla"))
                {
                    Myproduct.JumlaSoldQty += Myinvoice.Quantity;
                }
                else
                {
                    Myproduct.RetailSoldQty += Myinvoice.Quantity;
                }
                _ = productService.UpdateProduct(Myproduct).Result;
                if (response > 0)
                {
                    await Shell.Current.DisplayAlert("Congratulations!", "The Invoice has been successfully Updated", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Oops!", "Failed To Update the Invoice", "OK");

                }
                Myinvoice.CreatedTime = DateTime.Now.TimeOfDay;
            }
            else
            {   Myproduct.TotalQuantity += Myinvoice.Quantity;
                if (Myinvoice.Category.Equals("Jumla"))
                {
                    Myproduct.JumlaSoldQty += Myinvoice.Quantity;
                }
                else
                {
                    Myproduct.RetailSoldQty += Myinvoice.Quantity;
                }
                _ = productService.UpdateProduct(Myproduct).Result;
                var response = await invoiceService.AddInvoice(new Invoice()
                {
                    Name= Myproduct.Name,
                    IsExpense=Myproduct.IsExpense,                    
                    ProductUnit=Myproduct.ProductUnit,
                    Cost= Myinvoice.Cost,
                    Quantity= Myinvoice.Quantity,
                    ProductCost= Myinvoice.ProductCost,
                    CreatedTime = Myinvoice.CreatedTime,
                    CreatedDate = DateTime.Now, 
                    LastUpdatedDate= Myinvoice.CreatedDate.Date + DateTime.Today.Add(Myinvoice.CreatedTime).TimeOfDay,
                    Category = Myinvoice.Category, 
                    IsNotPaid=Myinvoice.IsNotPaid,
                    CustomerName=Myinvoice.CustomerName,
                    IsPaid=Myinvoice.IsPaid,
                    

                });
                if (response > 0)
                {
                    await Shell.Current.DisplayAlert("Congratulations!", "The Invoice has been successfully Added", "OK");

                }
                else
                {
                    await Shell.Current.DisplayAlert("Oops!", "Failed To Add the Invoice", "OK");

                }
                Myinvoice.CreatedTime = DateTime.Now.TimeOfDay;

            }
        }








       


    }
}
