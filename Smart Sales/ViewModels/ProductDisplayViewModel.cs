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
    [QueryProperty(nameof(MyProduct), "Product Detail")]
    public partial class ProductDisplayViewModel : ObservableObject
    {
        [ObservableProperty]
        public Product myProduct;

        [ObservableProperty]
        public double jumlaQtyLeo;

        [ObservableProperty]
        public double retailQtyLeo;

        public ObservableCollection<Invoice> ListInvoices { get; set; } = new ObservableCollection<Invoice>();
        public ObservableCollection<InvoiceList> dates { get; set; }
        private readonly IInvoiceService invoiceService;
        private readonly IProductService productService;
        private readonly IStoreService storeService;

        public ProductDisplayViewModel(IProductService productService, IInvoiceService invoiceService, IStoreService storeService)
        {

            MyProduct = new Product();
            this.productService = productService;
            this.invoiceService = invoiceService;
            dates = CreateDateCollection();
            ListInvoices = new ObservableCollection<Invoice>();
            SortAllInvoices(new InvoiceList() { InvoiceDate = DateTime.Now });
            this.storeService = storeService;
        }

        public ObservableCollection<InvoiceList> CreateDateCollection()
        {
            ObservableCollection<InvoiceList> dateCollection = new ObservableCollection<InvoiceList>();

            // Start from the current month and go back to the last 12 months
            DateTime currentDate = DateTime.Now;
            for (int i = 0; i < 12; i++)
            {
                dateCollection.Add(new InvoiceList()
                {
                    IDate = currentDate.ToString("MMM dd yyyy"),
                    InvoiceDate = currentDate
                });
                currentDate = currentDate.AddMonths(-1);
            }

            dateCollection = new ObservableCollection<InvoiceList>(dateCollection.OrderBy(i => i.InvoiceDate));

            return dateCollection;
        }

        [RelayCommand]
        public void SortAllInvoices(InvoiceList list)
        {   ListInvoices.Clear();

            var myinvoices = invoiceService.GetAllInvoices().Result;

            var invoicelist = myinvoices.Where(i => i.LastUpdatedDate.Month == list.InvoiceDate.Month && i.LastUpdatedDate.Year == list.InvoiceDate.Year && i.Name == MyProduct.Name)
                                                                       .OrderByDescending(i => i.LastUpdatedDate);

            foreach (var item in invoicelist)
            {
                ListInvoices.Add(item);
            }
        }

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task GoToProductDetails(Product product)
        {
            var taskdict = new Dictionary<string, object>();
            taskdict.Add("Product Detail", product);
            await Shell.Current.GoToAsync(nameof(ProductsDetailsPage), true, taskdict);
        }

        [RelayCommand]
        public async Task GoToProductHistory(Product product)
        {
            var taskdict = new Dictionary<string, object>();
            taskdict.Add("Product Detail", product);
            await Shell.Current.GoToAsync(nameof(ProductHistoryPage), true, taskdict);
        }

        [RelayCommand]
        public async Task DeleteProduct(Product product)
        {
            bool ans = await Shell.Current.DisplayAlert("DELETE PRODUCT", "Would you like to delete this product?", "Yes", "No");
            if (ans)
            {
                var delresponse = await productService.DeleteProduct(product);
                if (delresponse > 0)
                {
                    await Shell.Current.DisplayAlert("Congratulations!", "The Product has been successfully Deleted", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                                    
                else
                {
                    await Shell.Current.DisplayAlert("Oops!", "Failed To Delete the Product", "OK");
                    return;
                }
            }
           
        }
        
        public async Task AddProductQuantity()
        {
            var value = await Shell.Current.DisplayPromptAsync("ENTER THE QUANTITY TO ADD:", "Enter a positive value of the Quantity without commas","OK","Cancel","Product Quantity To Be Added", 1000, Keyboard.Numeric,"0");
            int num = 0;
            try
            {
                 num = int.Parse(value);
            }
            catch (Exception )
            {
                return;
            }
            
            if(num > 0) 
            {
                MyProduct.AvailableQuantity += num;
                _ = productService.UpdateProduct(MyProduct).Result;
                _ = storeService.AddProduction(new Production() { ProductName=MyProduct.Name, 
                                                                  ProductionDate=DateTime.Now,
                                                                  ProductQuantity=num,
                                                                  ProductUnit=MyProduct.ProductUnit}).Result;
                 await Shell.Current.DisplayAlert("ADDED SUCCESFULLY!", "The Product Quantity has been added and recorded", "OK");

                return;
            }
            else 
            {
                await Shell.Current.DisplayAlert("NEGATIVE OR ZERO NUMBER ", "Failed to Add the Product Quantity", "OK");
                return ;
            }
        
        }

        public void SortInvoicesonDate(DateTime date)
        {
            DateTime startdate = date.Date;
            DateTime enddate = startdate.AddHours(23).AddMinutes(59).AddSeconds(59);

            var invoices = invoiceService.GetAllInvoices().Result
                .Where(i=>i.LastUpdatedDate >= startdate && i.LastUpdatedDate <= enddate && i.Name == MyProduct.Name);

            var jumlainvoices = invoices.Where(i => i.Category.Equals("Jumla"));
            var retailinvoices = invoices.Where(i => i.Category.Equals("Retail"));

            JumlaQtyLeo = jumlainvoices.Sum(i => i.Quantity);
            RetailQtyLeo = retailinvoices.Sum(i => i.Quantity);


        }
    }
}
