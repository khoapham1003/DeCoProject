using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DeCo.Model;
namespace DeCo.ViewModel
{
    public class BillViewModel : BaseViewModel
    {
        private ObservableCollection<BillDetail> _bills;
        public ObservableCollection<BillDetail> bills { get { return _bills; } set { _bills = value; OnPropertyChanged(); } }

        private ObservableCollection<Order> _orders = new ObservableCollection<Order>();
        public ObservableCollection<Order> orders { get { return _orders; } set { _orders = value; OnPropertyChanged(); } }
        private Customer _Customer;
        public Customer Customer { get { return _Customer; } set { _Customer = value; OnPropertyChanged(); } }

        private Employee _Employee;
        public Employee Employee { get { return _Employee; } set { _Employee = value; OnPropertyChanged(); } }
        private decimal _Discount = 0;
        public decimal Discount { get { return _Discount; } set { _Discount = value; OnPropertyChanged(); } }
        private decimal _TotalPrice = 0;
        public decimal TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; OnPropertyChanged(); } }
        private BillDetail _SelectedBill;
        public BillDetail SelectedBill
        {
            get => _SelectedBill;
            set
            {
                _SelectedBill = value; OnPropertyChanged();
                if (_SelectedBill != null)
                {
                    using (var db = new DeCoDbContext())
                    {
                        orders.Clear();
                        var query = from b in db.Orders where (b.BillId == _SelectedBill.Id) select b;
                        foreach (var order in query)
                        {
                            orders.Add(order);
                        }
                        var query1 = from b in db.Customers where (b.Id == _SelectedBill.CusId) select b;
                        Customer = query1.FirstOrDefault();
                        var query2 = from b in db.Employees where (b.Id == _SelectedBill.EmployeeId) select b;
                        Employee = query2.FirstOrDefault();
                        if (Customer != null)
                        {
                            switch (Customer.MemberRank)
                            {
                                case "None":
                                    Discount = 0;
                                    break;
                                case "Silver":
                                    Discount = 5;
                                    break;
                                case "Golden":
                                    Discount = 10;
                                    break;
                                case "Platinum":
                                    Discount = 15;
                                    break;
                                case "Diamond":
                                    Discount = 20;
                                    break;
                                default:
                                    break;
                            }
                        }
                        else Discount = 0;
                        TotalPrice = (decimal)_SelectedBill.TotalMoney / (1 - Discount / 100);
                    }
                }
            }
        }

        private DateTime _Date=DateTime.Today.AddDays(-1);
        public DateTime Date { get { return _Date; } set { _Date = value; OnPropertyChanged(); } }
        private decimal _Revenue = 0;
        public decimal Revenue { get { return _Revenue; } set { _Revenue = value; OnPropertyChanged(); } }
        public ICommand DateChanged { get; set; }
        public ICommand cancelCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand Searching { get; set; }

        private string _SearchString;
        public string SearchString { get => _SearchString; set { _SearchString = value; loadList(); OnPropertyChanged(); } }

        public BillViewModel()
        {
            loadList();
            DateChanged = new RelayCommand<Object>((p) => { return true; },
                (p) =>
                {
                    decimal rev = 0;
                    using (var db = new DeCoDbContext())
                    {
                        var query1 = from b in bills where (((DateTime)b.BillDate).Date == Date.Date ) select b;
                        ObservableCollection<BillDetail> listOut = new ObservableCollection<BillDetail>(query1.ToList());
                        var query2 = from b in db.Products from c in db.Orders where (((DateTime)c.Bill.BillDate).Date == Date.Date && c.ProductId == b.Id) select b;
                        ObservableCollection<Product> listIn = new ObservableCollection<Product>(query2.ToList());

                        foreach (var item in listOut)
                        {
                            rev += (decimal)item.TotalMoney;//Money earned from Customer(already discounted)
                        }
                        foreach (var item in listIn)
                        {
                            var qr = from b in db.Orders where (b.ProductId == item.Id) select b;
                            decimal amount = (decimal)qr.FirstOrDefault().Amount;
                            rev -= (decimal)item.ImportPrice * amount;//Money pay for Import Products
                        }
                        Revenue = rev;
                    }
                });
            cancelCommand = new RelayCommand<Object>((p) => { return true; }, (p) => { ClearFields(); });
            DeleteCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedBill != null)
                    return true;
                return false;
            },
            (p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.BillDetails select b;
                    var bill = query.Where(x => x.Id == SelectedBill.Id).FirstOrDefault();
                    bill.Deleted = true;

                    var query1 = from b in db.Orders where (bill.Id == b.BillId) select b;
                    foreach(var order in query1)
                    {
                        order.Deleted = true;
                    }    
                    db.SaveChanges();
                    
                }
                bills.Remove(SelectedBill);
                ClearFields();
            });
            Searching = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>

            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.BillDetails select b;

                    if (string.IsNullOrEmpty(SearchString))
                    {
                        query = query.Where(x => x.Deleted != true);
                    }
                    else
                    {
                        query = query.Where(x => ((x.Id.ToString()).Contains(SearchString)) && x.Deleted != true);
                    }
                    ObservableCollection<BillDetail> list = new ObservableCollection<BillDetail>(query.ToList());
                    bills = list;
                }

            });
        }
        private void ClearFields()
        {
            Date = DateTime.Now.AddDays(-1);
            Revenue = 0;
            orders.Clear();
            Discount = 0;
            TotalPrice = 0;
            Customer = null;
            Employee = null;
            loadList();
            SelectedBill = null;
        }
        void loadList()
        {
            using (DeCoDbContext dbContext = new DeCoDbContext())
            {
                var query = from b in dbContext.BillDetails where (b.Deleted != true) select b;
                ObservableCollection<BillDetail> list = new ObservableCollection<BillDetail>(query.ToList());
                bills = list;
            }
        }
    }
}
