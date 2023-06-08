using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeCo.Model;
using System.Windows.Input;
using DeCo.Model;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows;

namespace DeCo.ViewModel
{
    public class OrderViewModel : BaseViewModel
    {


        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> orders { get { return _orders; } set { _orders = value; OnPropertyChanged(); } }
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> products { get { return _products; } set { _products = value; OnPropertyChanged(); } }
        private decimal _TotalPrice = 0;
        public decimal TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; OnPropertyChanged(); } }
        private decimal _TotalMoney = 0;
        public decimal TotalMoney { get { return _TotalMoney; } set { _TotalMoney = value; OnPropertyChanged(); } }

        private string _FindByEmpId;
        public string FindByEmpId { get { return _FindByEmpId; } set { _FindByEmpId = value; OnPropertyChanged(); } }
        private string _FindByPhoneNumber;
        public string FindByPhoneNumber { get { return _FindByPhoneNumber; } set { _FindByPhoneNumber = value; OnPropertyChanged(); } }
        private Customer _Customer;
        public Customer Customer { get { return _Customer; } set { _Customer = value; OnPropertyChanged(); } }

       // private Employee _Employee;
        // public Employee Employee { get { return _Employee; } set { _Employee = value; OnPropertyChanged(); } }
        private int _Id;
        public int Id { get { return _Id; } set { _Id = value; OnPropertyChanged(); } }
        private DateTime _BillDate = DateTime.Now;
        public DateTime BillDate { get { return _BillDate; } set { _BillDate = value; OnPropertyChanged(); } }
        private decimal _Discount = 0;
        public decimal Discount { get { return _Discount; } set { _Discount = value; OnPropertyChanged(); } }
        private Product _SelectedItem;
        public Product SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value; OnPropertyChanged();
                var temp = orders.Where(x => x.Product == value).FirstOrDefault();
                if (SelectedItem != null)
                {
                    if (temp == null)
                    {
                        var order = new Order();
                        order.Product = _SelectedItem;
                        order.ProductId = order.Product.Id;
                        order.Amount = 1;
                        order.TotalPrice = order.Amount * _SelectedItem.SalePrice;
                        order.Deleted = false;


                        TotalPrice += (decimal)order.TotalPrice;
                        TotalMoney = TotalPrice * (1 - Discount / 100);

                        orders.Add(order);
                    }
                    else
                    {
                        var order = orders.Where(x => x == temp).FirstOrDefault();
                        order.ProductId = order.Product.Id;
                        using (var db = new DeCoDbContext())
                        {
                            var query = from b in db.Products where (order.ProductId == b.Id) select b;
                            Product product = query.FirstOrDefault();
                            if (product.Quantity > order.Amount)
                            {
                                order.Amount += 1;
                                order.TotalPrice = order.Amount * _SelectedItem.SalePrice;
                                TotalPrice += (decimal)_SelectedItem.SalePrice;
                                TotalMoney = TotalPrice * (1 - Discount / 100);
                            }
                        }
                        CollectionViewSource.GetDefaultView(orders).Refresh();
                    }
                        SelectedItem = null;
                }
            }
        }
        public ICommand cancelCommand { get; set; }
        public ICommand paymentCommand { get; set; }
        public ICommand AmountIncreaseCommand { get; set; }
        public ICommand AmountDecreaseCommand { get; set; }
        public ICommand DeleteOrderCommand { get; set; }
        public ICommand ClearAllOrdersCommand { get; set; }
        public ICommand LoadedFindByChanged { get; set; }
        public ICommand Searching { get; set; }

        private string _SearchString;
        public string SearchString { get => _SearchString; set { _SearchString = value; loadList(); OnPropertyChanged(); } }

        public OrderViewModel()
        {
            loadList();
            orders = new ObservableCollection<Order>();
            using (var db = new DeCoDbContext())
            {
                int x = db.BillDetails.Count();
                Id = 8000 + x + 1;
            }
            cancelCommand = new RelayCommand<Object>((p) => { return true; }, (p) => { ClearFields(); });
            paymentCommand = new RelayCommand<Object>((p) =>
            {
                if (string.IsNullOrEmpty(FindByPhoneNumber) || TotalPrice == 0 || orders.Count <= 0 || Const.ActiveUser.EmployeeId == null)
                    return false;
                return true;
            }, (p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    var bill = new BillDetail()
                    {
                        Id = Id,
                        CusId = Customer.Id,
                        EmployeeId = Const.ActiveUser.Employee.Id,
                        BillDate = DateTime.Now,
                        TotalMoney = TotalMoney,
                        Deleted = false,
                    };
                    db.BillDetails.Add(bill);
                    db.SaveChanges();
                    foreach (var order in orders)
                    {
                        int y = db.Orders.Count();
                        var orderToDb = new Order()
                        {
                            Id = 7000 + y + 1,
                            BillId = Id,
                            ProductId = order.ProductId,
                            Amount = order.Amount,
                            TotalPrice = order.TotalPrice,
                            Deleted = false,
                        };
                        db.Orders.Add(orderToDb);
                        //product quantity decrease
                        var queryQuantityDes = from b in db.Products where b.Id == order.ProductId select b;
                        Product product = queryQuantityDes.FirstOrDefault();
                        product.Quantity-=order.Amount;
                        db.SaveChanges();
                    }
                    //customer spending
                    var querySpend = from b in db.Customers where b.Id == Customer.Id && b.Deleted == false select b;
                    Customer customer= querySpend.FirstOrDefault();
                    customer.Spending += TotalPrice;
                    customer.MemberRank = customer.Spending < 5000000 ? "None" :
                    customer.Spending < 15000000 ? "Silver" :
                    customer.Spending < 30000000 ? "Golden" :
                    customer.Spending < 50000000 ? "Platinum" : "Diamond";
                    db.SaveChanges();
                    //
                    ClearFields();
                    int x = db.BillDetails.Count();
                    Id = 8000 + x + 1;
                }
            });
            AmountIncreaseCommand = new RelayCommand<Order>((p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Products where (p.ProductId == b.Id) select b;
                    Product product = query.FirstOrDefault();

                    if (product.Quantity > p.Amount)
                        return true;
                    return false;
                }
            }, (p) =>
            {
                p.Amount++;
                TotalPrice -= (decimal)p.TotalPrice;
                p.TotalPrice = p.Amount * p.Product.SalePrice;
                TotalPrice += (decimal)p.TotalPrice;
                TotalMoney = TotalPrice * (1 - Discount / 100);
                CollectionViewSource.GetDefaultView(orders).Refresh();

            });
            AmountDecreaseCommand = new RelayCommand<Order>((p) =>
            {
                if (p.Amount > 0)
                    return true;
                return false;
            }, (p) =>
             {
                 p.Amount--;
                 if (p.Amount > 0)
                 {
                     TotalPrice -= (decimal)p.TotalPrice;
                     p.TotalPrice = p.Amount * p.Product.SalePrice;
                     TotalPrice += (decimal)p.TotalPrice;
                     TotalMoney = TotalPrice * (1 - Discount / 100);
                 }
                 else
                 {
                     TotalPrice -= (decimal)p.TotalPrice;
                     TotalMoney = TotalPrice * (1 - Discount / 100);
                     orders.Remove(p);
                 }
                 CollectionViewSource.GetDefaultView(orders).Refresh();

             });
            DeleteOrderCommand = new RelayCommand<Order>((p) =>
            {
                return true;
            }, (p) =>
             {
                 TotalPrice -= (decimal)p.TotalPrice;
                 orders.Remove(p);
                 TotalMoney = TotalPrice * (1 - Discount / 100);
             });
            LoadedFindByChanged = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
             {
                /* //employee
                 {
                     if (string.IsNullOrEmpty(FindByEmpId))
                     {
                         Employee = null;
                     }
                     else
                     {
                         using (var db = new DeCoDbContext())
                         {
                             var query = from b in db.Employees select b;
                             // int empId = Int32.Parse(FindByEmpId);
                             Employee = query.Where(x => ((x.Id.ToString()).Contains(FindByEmpId))).FirstOrDefault();
                         }
                     }
                 }*/
                 //customer
                 {
                     if (string.IsNullOrEmpty(FindByPhoneNumber))
                     {
                         Customer = null;
                     }
                     else
                     {
                         using (var db = new DeCoDbContext())
                         {
                             var query = from b in db.Customers select b;
                             Customer = query.Where(x => (x.PhoneNumber.Trim()).Contains((FindByPhoneNumber.Trim()))).FirstOrDefault();
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
                         }
                     }
                     TotalMoney = TotalPrice * (1 - Discount / 100);
                 }
             });
            Searching = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>

            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Products select b;

                    if (string.IsNullOrEmpty(SearchString))
                    {
                        query = query.Where(x => x.Deleted != true);
                    }
                    else
                    {
                        query = query.Where(x => (x.DisplayName.Trim()).Contains((SearchString.Trim())) && x.Deleted != true);
                    }
                    ObservableCollection<Product> list = new ObservableCollection<Product>(query.ToList());
                    products = list;
                }

            });
        }
        private void ClearFields()
        {
            Customer = null;
            //Employee = null;
            TotalMoney = TotalPrice = 0;
            FindByPhoneNumber = string.Empty;
            FindByEmpId = string.Empty;
            SelectedItem = null;
            loadList();
            orders.Clear();

        }
        void loadList()
        {
            using (DeCoDbContext dbContext = new DeCoDbContext())
            {
                var query = from b in dbContext.Products where (b.Deleted != true && b.Quantity>0) select b;
                ObservableCollection<Product> list = new ObservableCollection<Product>(query.ToList());
                products = list;
            }

        }

    }
}
