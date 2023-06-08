using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DeCo.Model;
using System.Collections.ObjectModel;

namespace DeCo.ViewModel
{
    public class ProductViewModel : BaseViewModel
    {
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> products { get { return _products; } set { _products = value; OnPropertyChanged(); } }
        private Product _SelectedItem;
        public Product SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    DisplayType = SelectedItem.DisplayType;
                    Company = SelectedItem.Company;
                    Quantity = (int)SelectedItem.Quantity;
                    ImportPrice = (decimal)SelectedItem.ImportPrice;
                    SalePrice = (decimal)SelectedItem.SalePrice;
                }
            }
        }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }

        private string _DisplayType;
        public string DisplayType { get => _DisplayType; set { _DisplayType = value; OnPropertyChanged(); } }
        private string _Company;
        public string Company { get => _Company; set { _Company = value; OnPropertyChanged(); } }
        private int _Quantity;
        public int Quantity { get => _Quantity; set { _Quantity = value; OnPropertyChanged(); } }
        private decimal _ImportPrice;
        public decimal ImportPrice { get => _ImportPrice; set { _ImportPrice = value; OnPropertyChanged(); } }
        private decimal _SalePrice;
        public decimal SalePrice { get => _SalePrice; set { _SalePrice = value; OnPropertyChanged(); } }
        public ICommand addCommand { get; set; }
        public ICommand editCommand { get; set; }
        public ICommand deleteCommand { get; set; }
        public ICommand cancelCommand { get; set; }
        public ICommand Searching { get; set; }

        private string _SearchString;
        public string SearchString { get => _SearchString; set { _SearchString = value; loadList(); OnPropertyChanged(); } }

        public ProductViewModel()
        {
            loadList();
            addCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(DisplayType) || string.IsNullOrEmpty(Company) || (Quantity <0)|| (ImportPrice <= 0) || (SalePrice <= 0))
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    int x = db.Products.Count();
                    var product = new Product()
                    {
                        Id = 9000 + x + 1,
                        DisplayName = DisplayName,
                        DisplayType = DisplayType,
                        Company = Company,
                        Quantity = Quantity,
                        ImportPrice = ImportPrice,
                        SalePrice = SalePrice,
                        Deleted = false,
                    };
                    db.Products.Add(product);
                    db.SaveChanges();
                }
                ClearFields();
                SelectedItem = null;
            });
            editCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(DisplayType) || string.IsNullOrEmpty(Company) || (Quantity < 0) || (ImportPrice <= 0) || (SalePrice <= 0))
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Products
                                select b;
                    var product = query.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
                    product.DisplayName = DisplayName;
                    product.DisplayType = DisplayType;
                    product.Company = Company;
                    product.Quantity = Quantity;
                    product.ImportPrice = ImportPrice;
                    product.SalePrice = SalePrice;
                    db.SaveChanges();
                }
                SelectedItem.DisplayName = DisplayName;
                SelectedItem.DisplayType = DisplayType;
                SelectedItem.Company = Company;
                SelectedItem.Quantity = Quantity;
                SelectedItem.ImportPrice = ImportPrice;
                SalePrice = SalePrice;
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
                    var query = from b in db.Products
                                select b;
                    var product = query.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
                    product.Deleted = true;
                    db.SaveChanges();

                }
                products.Remove(SelectedItem);
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
            DisplayName = DisplayType = Company = null;
            Quantity = 0;
            ImportPrice = SalePrice = 0;
            loadList();
            SelectedItem = null;

        }
        void loadList()
        {

            using (DeCoDbContext dbContext = new DeCoDbContext())
            {
                var query = from b in dbContext.Products where (b.Deleted != true) select b;
                ObservableCollection<Product> list = new ObservableCollection<Product>(query.ToList());
                products = list;
            }
        }
    }
}
