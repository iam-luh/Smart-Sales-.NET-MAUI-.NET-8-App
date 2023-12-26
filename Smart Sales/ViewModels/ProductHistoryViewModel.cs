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
    [QueryProperty(nameof(Myproduct), "Product Detail")]
    public partial class ProductHistoryViewModel : ObservableObject
    {
        public ObservableCollection<Production> ListInvoices { get; set; } = new ObservableCollection<Production>();
        public ObservableCollection<InvoiceList> dates { get; set; } = new ObservableCollection<InvoiceList>();

        [ObservableProperty]
        public Product myproduct = new Product();

        private readonly IStoreService storeService;

        public ProductHistoryViewModel(IStoreService storeService)
        {
            this.storeService = storeService;
            dates = CreateDateCollection();     
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
        {
            ListInvoices.Clear();

            var myinvoices = storeService.GetAllProductions().Result;

            var productionslist = myinvoices.Where(i => i.ProductionDate.Month == list.InvoiceDate.Month && i.ProductionDate.Year == list.InvoiceDate.Year && i.ProductName == Myproduct.Name)
                                                                       .OrderByDescending(i => i.ProductionDate);

            foreach (var item in productionslist)
            {
                ListInvoices.Add(item);
            }
        }

        [RelayCommand]
        public async Task GoBack()
        {
            await Shell.Current.GoToAsync("..");
        }

    }
}
