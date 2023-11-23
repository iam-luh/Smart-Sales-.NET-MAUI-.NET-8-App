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

        public ProductDisplayViewModel(IProductService productService, IInvoiceService invoiceService)
        {
            MyProduct = new Product();
            this.productService = productService;
            this.invoiceService = invoiceService;
            dates = CreateDateCollection();            
            ListInvoices = new ObservableCollection<Invoice>();
            SortAllInvoices(new InvoiceList() { InvoiceDate = DateTime.Now });

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

            var invoicelist = myinvoices.Where(i => i.CreatedDate.Month == list.InvoiceDate.Month && i.CreatedDate.Year == list.InvoiceDate.Year && i.Name == MyProduct.Name)
                                                                       .OrderByDescending(i => i.CreatedDate);

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
