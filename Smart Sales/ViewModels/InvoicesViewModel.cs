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
    public partial class InvoicesViewModel : ObservableObject
    {
        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public ObservableCollection<Invoice> ListInvoices { get; set; }  = new ObservableCollection<Invoice>();
        public ObservableCollection<InvoiceList> dates { get; set; }
        private readonly IInvoiceService invoiceService;
        private readonly IProductService productService;
        public InvoicesViewModel(IInvoiceService invoiceService,IProductService productService) 
        {
            this.invoiceService = invoiceService;
            this.productService = productService;
            dates = CreateDateCollection();            
            ListInvoices = new ObservableCollection<Invoice>();
            SortAllInvoices(new InvoiceList() { InvoiceDate = DateTime.Now });
        }

        public  ObservableCollection<InvoiceList> CreateDateCollection()
        {
            ObservableCollection<InvoiceList> dateCollection = new ObservableCollection<InvoiceList>();

            // Start from the current month and go back to the last 12 months
            DateTime currentDate = DateTime.Now;
            for (int i = 0; i < 12; i++)
            {
                dateCollection.Add(new InvoiceList() { IDate=currentDate.ToString("MMM dd yyyy"),
                                                       InvoiceDate=currentDate});
                currentDate = currentDate.AddMonths(-1);
            }           
            
           dateCollection = new ObservableCollection<InvoiceList>(dateCollection.OrderBy(i => i.InvoiceDate));

            return dateCollection;
            
        }


      

        [RelayCommand]
        public void SortAllInvoices(InvoiceList list )
        {

            ListInvoices.Clear();

            var invoices =  invoiceService.GetAllInvoices().Result;

            var invoicelist = invoices.Where(i => i.LastUpdatedDate.Month == list.InvoiceDate.Month && i.LastUpdatedDate.Year == list.InvoiceDate.Year)
                                                                       .OrderByDescending(i => i.LastUpdatedDate);

            foreach (var item in invoicelist)
            {
                ListInvoices.Add(item);
            }            
        }

        [RelayCommand]
        public async Task DeleteInvoice(Invoice myinvoice)
        {
            bool ans = await Shell.Current.DisplayAlert("DELETE INVOICE?", "Would you like to delete this Invoice?", "Yes", "No");
            if (ans)
            {
                var delresponse = await invoiceService.DeleteInvoice(myinvoice);
                if (delresponse > 0)
                {
                    ListInvoices.Remove(myinvoice);                
                    var products= await productService.GetAllProducts();

                    foreach (var product in products)
                    {
                        if (myinvoice.Name.Equals(product.Name))
                        {
                            product.TotalQuantity -= myinvoice.Quantity;

                            if (myinvoice.Category.Equals("Jumla"))
                            {
                                product.JumlaSoldQty -= myinvoice.Quantity;
                            }
                            else
                            {
                                product.RetailSoldQty -= myinvoice.Quantity;
                            }
                            await productService.UpdateProduct(product);
                            break;
                        }

                       
                    }
                }
            }
            else
            {
                return;
            }
        }


        [RelayCommand]
        public async Task GoToInvoiceDetails(Invoice myinvoice)
        {   myinvoice.CantEdit = true;
            var taskdict = new Dictionary<string, object>();
            taskdict.Add("Invoice Detail", myinvoice);
            await Shell.Current.GoToAsync(nameof(AddInvoicePage),  taskdict);
        }




    }
}
