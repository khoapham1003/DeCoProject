using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DeCo.Model;
namespace DeCo.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        private ObservableCollection<Customer> _customers;
        public ObservableCollection<Customer> customers { get => _customers; set { _customers = value; OnPropertyChanged(); } }
        private Customer _SelectedItem;
        public Customer SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    PhoneNumber = SelectedItem.PhoneNumber;
                    Email = SelectedItem.Email;
                    MemberRank = SelectedItem.MemberRank;
                    Spending = (decimal)SelectedItem.Spending;
                    MemberDate = (DateTime)SelectedItem.MemberDate;
                    Nationality = SelectedItem.Nationality;
                }
            }
        }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        private DateTime _MemberDate = DateTime.Now;
        public DateTime MemberDate { get => _MemberDate; set { _MemberDate = value; OnPropertyChanged(); } }
        private string _PhoneNumber;
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }
        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }
        private string _Nationality;
        public string Nationality { get => _Nationality; set { _Nationality = value; OnPropertyChanged(); } }
        private string _MemberRank = "None";
        public string MemberRank { get => _MemberRank; set { _MemberRank = value; OnPropertyChanged(); } }
        private decimal _Spending = 0;
        public decimal Spending { get => _Spending; set { _Spending = value; OnPropertyChanged(); } }
        public ICommand addCommand { get; set; }
        public ICommand editCommand { get; set; }
        public ICommand deleteCommand { get; set; }
        public ICommand cancelCommand { get; set; }
        public ICommand Searching { get; set; }

        private string _SearchString;
        public string SearchString { get => _SearchString; set { _SearchString = value; loadList(); OnPropertyChanged(); } }

        public CustomerViewModel()
        {
            loadList();
            addCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Nationality))
                {
                    return false;
                }
                /* var list = DataProvider.Instance.DB.Customers.Where(x => x.CCCD == CCCD && x.Deleted == false);
                 if (list == null || list.Count() != 0)
                 {
                     return false;
                 }*/
                return true;
            }, (p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    int x = db.Customers.Count();
                    var customer = new Customer()
                    {
                        Id = 3000 + x + 1,
                        DisplayName = DisplayName,
                        MemberDate = DateTime.Now,
                        PhoneNumber = PhoneNumber,
                        Email = Email,
                        Spending = Spending,
                        MemberRank = MemberRank,
                        Nationality = Nationality,
                        Deleted = false,
                    };
                    db.Customers.Add(customer);
                    db.SaveChanges();
                }
                ClearFields();
                SelectedItem = null;
            });
            editCommand = new RelayCommand<object>((p) =>
             {
                 if (SelectedItem == null || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Nationality))
                 {
                     return false;
                 }
                 return true;
             }, (p) =>
             {
                 using (var db = new DeCoDbContext())
                 {
                     var query = from b in db.Customers
                                 select b;
                     var customer = query.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
                     customer.DisplayName = DisplayName;
                     customer.PhoneNumber = PhoneNumber;
                     customer.Email = Email;
                     customer.Nationality = Nationality;
                     db.SaveChanges();
                 }
                 SelectedItem.DisplayName = DisplayName;
                 SelectedItem.PhoneNumber = PhoneNumber;
                 SelectedItem.Email = Email;
                 SelectedItem.Nationality = Nationality;
                 OnPropertyChanged("SelectedItem");
                 loadList();
                 SelectedItem = null;
             });
            deleteCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Customers
                                select b;
                    var customer = query.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
                    customer.Deleted = true;
                    db.SaveChanges();

                }
                customers.Remove(SelectedItem);
                ClearFields();
            });
            cancelCommand = new RelayCommand<Object>((p) => { return true; }, (p) => { ClearFields(); });
            Searching = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>

                {
                    using (var db = new DeCoDbContext())
                    {
                        var query = from b in db.Customers select b;

                        if (string.IsNullOrEmpty(SearchString))
                        {
                            query = query.Where(x => x.Deleted != true);
                        }
                        else
                        {
                            query = query.Where(x => (x.DisplayName.Trim()).Contains((SearchString.Trim())) && x.Deleted!=true);
                        }
                        ObservableCollection<Customer> list = new ObservableCollection<Customer>(query.ToList());
                        customers = list;
                    }

                });
        }
        private void ClearFields()
        {
            DisplayName = PhoneNumber = Email = MemberRank = Nationality = null;
            Spending = 0;
            MemberDate = DateTime.Now;
            SelectedItem = null;
            loadList();
        }
        void loadList()
        {

            using (DeCoDbContext dbContext = new DeCoDbContext())
            {
                var query = from b in dbContext.Customers where (b.Deleted != true) select b;
                ObservableCollection<Customer> list = new ObservableCollection<Customer>(query.ToList());
                customers = list;
            }

        }
    }
}
