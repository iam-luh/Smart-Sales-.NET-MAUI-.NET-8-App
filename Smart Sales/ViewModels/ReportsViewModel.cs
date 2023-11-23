﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smart_Sales.Models;
using Smart_Sales.Services;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;

namespace Smart_Sales.ViewModels
{
    public partial class ReportsViewModel : ObservableObject
    {
        public ObservableCollection<Invoice> NetIncome { get; set; }= new ObservableCollection<Invoice>();
        public ObservableCollection<Invoice> Expense { get; set; } = new ObservableCollection<Invoice>();
        public ObservableCollection<Invoice> Income { get; set; } = new ObservableCollection<Invoice>();
        public ObservableCollection<InvoiceList> dates { get; set; } = new ObservableCollection<InvoiceList>();

        [ObservableProperty]
        public double mynetincome;

        [ObservableProperty]
        public double myexpense;

        [ObservableProperty]
        public double myincome;

        [ObservableProperty]
        public string mostsoldprd;

        [ObservableProperty]
        public string leastsoldprd;

        [ObservableProperty]
        public string mostsoldsrvc;

        [ObservableProperty]
        public string leastsoldsrvc;

        [ObservableProperty]
        public bool ismonth;

        private readonly IInvoiceService invoiceService;
        private readonly IProductService productService;
        public ReportsViewModel(IInvoiceService invoiceService, IProductService productService)
        {
            this.invoiceService = invoiceService;
            this.productService = productService;
            dates = CreateDateCollection();
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
                    IDate = currentDate.Year == DateTime.Now.Year || currentDate.Year == DateTime.Now.AddYears(-1).Year ?
                     currentDate.Year == DateTime.Now.AddYears(-1).Year ? "Last Year" : "This Year" : currentDate.ToString("yyyy"),
                    InvoiceDate = currentDate
                }) ;
                currentDate = currentDate.AddYears(-1);
            }

            dateCollection = new ObservableCollection<InvoiceList>(dateCollection.OrderBy(i => i.InvoiceDate));

            return dateCollection;

        }

        [RelayCommand]
        public void SortallData(InvoiceList date)
        {
            var invoices = invoiceService.GetAllInvoices().Result.Where(i=>i.LastUpdatedDate.Year == date.InvoiceDate.Year);
            //Get all the invoices from the database and then group the invoices which are income 
            var incomes = invoices.Where(i => i.IsExpense is false );
            var groupincomes = incomes.GroupBy(i => i.LastUpdatedDate.Month);
            Income.Clear();
            foreach ( var group in groupincomes )
            {
                double sum = group.Sum(i => i.Cost);

                Income.Add(new Invoice
                {
                    LastUpdatedDate = group.First().LastUpdatedDate, // Set the date as desired
                    Cost = sum,
                    Name = group.First().LastUpdatedDate.ToString("MMM")
                });
            }
            Income.OrderBy(i => i.LastUpdatedDate);

            //Get all the invoices from the database and then group the invoices which are expenses

            var expenses = invoices.Where(i => i.IsExpense is true);
            var groupexpenses = expenses.GroupBy(i =>i.LastUpdatedDate.Month);
            Expense.Clear();
            foreach( var group in groupexpenses)
            {
                double sum = group.Sum(i => i.Cost);

                Expense.Add(new Invoice
                {
                    LastUpdatedDate = group.First().LastUpdatedDate, // Set the date as desired
                    Cost = sum,
                    Name = group.First().LastUpdatedDate.ToString("MMM")
                });
            }
            Expense.OrderBy(i => i.LastUpdatedDate);

            NetIncome.Clear();
            for (int i = 12; i>=1; i--)
            {
                
                double totalnetincome = 0,income = 0 ,expense = 0;
                var incomelist = Income.Where(x=>x.LastUpdatedDate.Month == i);
                var expenselist = Expense.Where(x=>x.LastUpdatedDate.Month==i);

                if(incomelist.Count() == 1) 
                { 
                    income = incomelist.SingleOrDefault().Cost;
                }

                if (expenselist.Count() == 1)
                {
                    expense = expenselist.SingleOrDefault().Cost;
                }

                totalnetincome = income - expense;
                //Adding a new Invoice in the observable collection and setting the date of the invoice.
                NetIncome.Add(new Invoice
                {
                    LastUpdatedDate = DateTime.Now.AddMonths(i-DateTime.Now.Month), // Set the date as desired
                    Cost = totalnetincome,
                    Name = DateTime.Now.AddMonths(i - DateTime.Now.Month).ToString("MMM")
                });
            }
            // This displays the value for the netincome, income and expenses
            Myincome = Income.Sum(x => x.Cost);
            Myexpense = Expense.Sum(x => x.Cost);
            Mynetincome = NetIncome.Sum(x => x.Cost);

        }

        [RelayCommand]
        public void SortProductData(InvoiceList list)
        {
            var invoices = invoiceService.GetAllInvoices().Result.Where(x=>x.LastUpdatedDate.Year == list.InvoiceDate.Year);

            var groupedinvoices = invoices.GroupBy(x => x.Name);

            double qty = 0;
            int num = 1;
            //This loop passes through all the products 
            foreach (var group in groupedinvoices)
            {              
                double sum = group.Sum(x => x.Quantity);

                //This finds the most sold product
               if((sum > qty || num is 1)&& !group.FirstOrDefault().IsExpense)
               {
                    qty = sum;
                    Mostsoldprd = group.Key;
               }

               //This finds the least sold product
                if ((sum < qty || num is 1) && !group.FirstOrDefault().IsExpense)
                {
                    qty = sum;
                    Leastsoldprd = group.Key;
                }

                //This finds the Most sold service
                if ((sum > qty || num is 1) && group.FirstOrDefault().IsExpense)
                {
                    qty = sum;
                    Mostsoldsrvc = group.Key;
                }

                //This finds then Least sold service
                if ((sum < qty || num is 1) && group.FirstOrDefault().IsExpense)
                {
                    qty = sum;
                    Leastsoldsrvc = group.Key;
                }

                ++num;
            }
            num = 1;
            qty = 0;
        }


    }
}
