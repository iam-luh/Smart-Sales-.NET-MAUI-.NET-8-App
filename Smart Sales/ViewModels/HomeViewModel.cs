using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Smart_Sales.Models;
using Smart_Sales.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Sales.ViewModels
{
    public partial class HomeViewModel: ObservableObject
    {
        public ObservableCollection<Invoice> Invoices {get; set;} = new ObservableCollection<Invoice>();



        [ObservableProperty]
        public double grossincome;

        [ObservableProperty]
        public double totalexpenses;

        [ObservableProperty]
        public double netincome;

        [ObservableProperty]
        public double pergrossincome;

        [ObservableProperty]
        public double pertotalexpenses;

        [ObservableProperty]
        public double pernetincome;

        [ObservableProperty]
        public Color colorgrossincome;

        [ObservableProperty]
        public Color colornetincome;

        [ObservableProperty]
        public Color colortotalexpenses;

        public ObservableCollection<Invoice> Collector { get; set;} = new ObservableCollection<Invoice>();

        private  readonly IInvoiceService invoiceService;
        public HomeViewModel(IInvoiceService invoiceService) 
        {
          this.invoiceService = invoiceService;
          GetMostRecentInvoices();
        }

        
        

        public void GetMostRecentInvoices( )
        {
            var invoices =  invoiceService.GetAllInvoices().Result;
            // Sort the invoices by CreatedDate in descending order and take the top 'count' invoices
             var mylist = invoices.OrderByDescending(i => i.LastUpdatedDate).Take(3);

            Collector.Clear();
            foreach ( var item in mylist )
            {
                Collector.Add(item); 
            }           
        }

        [RelayCommand]
        public void FilterInvoicesForWeek(DateTime date)
        {
            DateTime startDate = GetStartDateForWeek(date);
            DateTime endDate = startDate.AddDays(6); // Sunday is 6 days ahead of Monday
            endDate = endDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            var MyInvoices = invoiceService.GetAllInvoices().Result;
            var invoiceslist = new ObservableCollection<Invoice>(
                MyInvoices.Where(invoice => startDate <= invoice.LastUpdatedDate  && invoice.LastUpdatedDate <= endDate)
                        .OrderBy(invoice => invoice.LastUpdatedDate));

            var newInvoices = new ObservableCollection<Invoice>();
            var groupedInvoices = invoiceslist.GroupBy(invoice =>
                CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(invoice.LastUpdatedDate.DayOfWeek));

            foreach (var dayGroup in groupedInvoices)
            {
                // Calculate the sum of prices for invoices on this day of the week
                double sumOfPrices = dayGroup.Sum(invoice => invoice.Cost);

                // Create a new invoice for this day of the week
                newInvoices.Add(new Invoice
                {
                    LastUpdatedDate = dayGroup.First().LastUpdatedDate, // Set the date as desired
                    Cost = sumOfPrices,
                    Name = dayGroup.First().LastUpdatedDate.ToString("ddd")
                });
            }

            Invoices.Clear();
            foreach( var item in newInvoices)
            {
                Invoices.Add(item);
            }
        }

        [RelayCommand]
        [Obsolete]
        public void CalculateWeekly()
        {
            DateTime startDate = GetStartDateForWeek(DateTime.Now);
            DateTime endDate = startDate.AddDays(6);
            var MyInvoices = invoiceService.GetAllInvoices().Result;

            var invoiceslist = new ObservableCollection<Invoice>(
               MyInvoices.Where(invoice => startDate <= invoice.LastUpdatedDate && invoice.LastUpdatedDate <= endDate));

            var invoiceslist1 = new ObservableCollection<Invoice>(
               MyInvoices.Where(invoice => startDate.AddDays(-7) <= invoice.LastUpdatedDate && invoice.LastUpdatedDate <= endDate.AddDays(-7)));

            Totalexpenses = invoiceslist.Where(invoice => invoice.IsPaid && invoice.IsExpense)
            .Sum(invoice => invoice.Cost);

            Grossincome = invoiceslist.Where(invoice => invoice.IsPaid && !invoice.IsExpense)
            .Sum(invoice => invoice.Cost);

            Netincome = Grossincome - Totalexpenses;

            var num1 = invoiceslist1.Where(invoice => invoice.IsPaid && invoice.IsExpense)
            .Sum(invoice => invoice.Cost);

            var num2 = invoiceslist1.Where(invoice => invoice.IsPaid && !invoice.IsExpense)
           .Sum(invoice => invoice.Cost);            

            Pertotalexpenses = (Totalexpenses - num1)/num1 * (double)100;

            Pertotalexpenses = num1 == (double)0 && Totalexpenses == 0 ? 0 : Pertotalexpenses;

            Pertotalexpenses = num1 == (double)0 && Totalexpenses>0 ? 1000 : Pertotalexpenses;


            Pergrossincome = (Grossincome - num2)/num2 * (double)100;

            Pergrossincome = num2 == (double)0 && Grossincome == 0 ? 0 : Pergrossincome;

            Pergrossincome = num2 == (double)0 && Grossincome > 0 ? 1000 : Pergrossincome;

            Pernetincome = Pergrossincome - Pertotalexpenses;

            Colorgrossincome = Pergrossincome >= 0 ? Color.FromHex("#228C22") : Color.FromHex("#FF0000");

            Colornetincome = Pernetincome >= 0 ? Color.FromHex("#228C22") : Color.FromHex("#FF0000");

            Colortotalexpenses = Pertotalexpenses >= 0 ? Color.FromHex("FF0000") : Color.FromHex("#228C22");
        }



        // Define a method to get the start date for a specific week in a year and month
        public DateTime GetStartDateForWeek(DateTime date)
        {
            
            DateTime startdate = DateTime.Today;
            if(date.DayOfWeek == DayOfWeek.Sunday) 
            { 
                startdate = date.AddDays(-6).Date ;
            }
            else if (date.DayOfWeek == DayOfWeek.Monday)
            {
                startdate = date.AddDays(0).Date;

            }
            else if (date.DayOfWeek == DayOfWeek.Tuesday)
            {
                    startdate = date.AddDays(-1).Date;

            }
            else if (date.DayOfWeek == DayOfWeek.Wednesday)
            {
                startdate = date.AddDays(-2).Date;

            }
            else if (date.DayOfWeek == DayOfWeek.Thursday)
            {
                startdate = date.AddDays(-3).Date;

            }
            else if (date.DayOfWeek == DayOfWeek.Friday)
            {
                startdate = date.AddDays(-4).Date;

            }
            else 
            {
                startdate = date.AddDays(-5).Date;
            }

            return startdate;
        }

        

        

        // A METHOD WHICH RETURNS A WEEK NUMBER BUT IS COMMENTED OUT
        //public int GetWeekNumberOfMonthYear(DateTime date)
        //{
        //    // Define the culture (e.g., en-US) and the calendar you want to use (e.g., GregorianCalendar).
        //    CultureInfo culture = new CultureInfo("en-US");
        //    Calendar calendar = culture.DateTimeFormat.Calendar;

        //    // Calculate the week number within the month.
        //    int weekNumber = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
        //    return weekNumber;
        //}





}
}
