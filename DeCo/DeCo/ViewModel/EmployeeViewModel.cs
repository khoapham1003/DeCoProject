using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DeCo.Model;
using System.Collections.ObjectModel;


namespace DeCo.ViewModel
{
    internal class EmployeeViewModel : BaseViewModel
    {
        private ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> employees { get => _employees; set { _employees = value; OnPropertyChanged(); } }

        private Employee _SelectedItem;
        public Employee SelectedItem
        {
            get => _SelectedItem; set
            {
                _SelectedItem = value; OnPropertyChanged();
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    WorkRoleIndex = (SelectedItem.WorkRole == "Staff") ? 1 : 2;
                    PhoneNumber = SelectedItem.PhoneNumber;
                    Email = SelectedItem.Email;
                    GenderIndex = (SelectedItem.Gender == "Male") ? 1 : (SelectedItem.Gender == "Female") ? 2 : 3;
                    ContractDate = (DateTime)SelectedItem.ContractDate;
                    using (var db = new DeCoDbContext())
                    {
                        var query = from b in db.Users select b;
                        var user = query.Where(x => x.EmployeeId == SelectedItem.Id).FirstOrDefault();
                        if (user != null)
                        {
                            Username = user.Username;
                            Password = user.PasswordHash;
                        }
                        else
                        {
                            Username = Password = null;
                        }
                    }

                }
            }
        }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged(); } }
        private int _WorkRoleIndex;
        public int WorkRoleIndex { get => _WorkRoleIndex; set { _WorkRoleIndex = value; OnPropertyChanged(); } }
        private string _WorkRole;
        public string WorkRole { get => _WorkRole; set { _WorkRole = value; OnPropertyChanged(); } }
        private int _GenderIndex;
        public int GenderIndex { get => _GenderIndex; set { _GenderIndex = value; OnPropertyChanged(); } }
        private string _Gender;
        public string Gender { get => _Gender; set { _Gender = value; OnPropertyChanged(); } }
        private string _PhoneNumber;
        public string PhoneNumber { get => _PhoneNumber; set { _PhoneNumber = value; OnPropertyChanged(); } }
        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private DateTime _ContractDate = DateTime.Now;
        public DateTime ContractDate { get => _ContractDate; set { _ContractDate = value; OnPropertyChanged(); } }
        private string _Username;
        public string Username { get => _Username; set { _Username = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get => _Password; set { _Password = value; OnPropertyChanged(); } }

        public ICommand addCommand { get; set; }
        public ICommand editCommand { get; set; }
        public ICommand deleteCommand { get; set; }
        public ICommand changePasswordCommand { get; set; }
        public ICommand disconnectCommand { get; set; }
        public ICommand cancelCommand { get; set; }
        public ICommand Searching { get; set; }

        private string _SearchString;
        public string SearchString { get => _SearchString; set { _SearchString = value; loadList(); OnPropertyChanged(); } }
        public EmployeeViewModel()
        {
            loadList();
            addCommand = new RelayCommand<object>((p) =>
             {
                 if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Email) || GenderIndex == 0 || WorkRoleIndex == 0)
                     return false;
                 //username and password can null
                 if (!(string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)))
                 {
                     using (var db = new DeCoDbContext())
                     {
                         var query = from b in db.Users where (b.Username == Username && b.EmployeeId == null) select b;
                         if (query.Count() > 0)//username existed and empId null
                         {
                             //notify that Username existed and not used yet//may be use new password or old one
                             Password = query.First().PasswordHash;
                             return true;
                             //now username and password not null
                         }
                         else
                         {
                             var query1 = from b in db.Users where (b.Username == Username && b.EmployeeId != null) select b;
                             if (query1.Count() > 0)
                             {
                                 //notify that Username used
                                 return false;
                             }
                             else
                             {
                                 //username not existed
                                 return true;
                             }
                         }
                     }
                 }
                 else return false;
                 return true;
             }, (p) =>
             {
                 WorkRole = (WorkRoleIndex == 1) ? "Staff" : "Manager";
                 Gender = (GenderIndex == 1) ? "Male" : (GenderIndex == 2) ? "Female" : "Other";
                 using (var db = new DeCoDbContext())
                 {
                     int x = db.Employees.Count();
                     var employee = new Employee()
                     {
                         Id = 2000 + x + 1,
                         DisplayName = DisplayName,
                         WorkRole = WorkRole,
                         PhoneNumber = PhoneNumber,
                         Email = Email,
                         Gender = Gender,
                         ContractDate = ContractDate,
                         Deleted = false,
                     };
                     db.Employees.Add(employee);

                     if (Username != "" && Username != null)
                     {
                         var query = from b in db.Users where (b.Username == Username && b.EmployeeId == null) select b;
                         if (query.Count() > 0)//username existed and empId null
                         {
                             var user = from b in db.Users where (b.Username == Username) select b;
                             user.First().EmployeeId = employee.Id;
                         }
                         else
                         {
                             int y = db.Users.Count();
                             var user = new User()
                             {
                                 Id = 1000 + y + 1,
                                 Username = Username,
                                 PasswordHash = Password,
                                 EmployeeId = employee.Id,
                             };
                             db.Users.Add(user);
                         }
                     }

                     db.SaveChanges();
                 }
                 ClearFields();
                 loadList();
                 SelectedItem = null;
             });
            editCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Email) || GenderIndex == 0 || WorkRoleIndex == 0)
                {
                    return false;
                }
                if (SelectedItem == null)
                {
                    return false;
                }
                /*if (!(string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)))
                {
                    using (var db = new DeCoDbContext())
                    {
                        var query = from b in db.Users where (b.Username == Username && b.EmployeeId == null) select b;
                        if (query.Count() > 0)//username existed and empId null
                        {
                            //notify that Username existed and not used yet//may be use new password or old one
                            Password = query.First().PasswordHash;
                            return true;
                            //now username and password not null
                        }
                        else
                        {
                            return true;
                            //var query1 = from b in db.Users where (b.Username == Username && b.EmployeeId != null) select b;
                            //if (query1.Count() > 0)
                            //{
                            //    //notify that Username used
                            //    return false;
                            //}
                            //else
                            //{
                            //    //username not existed
                            //    return true;
                        }
                    }
                }
                else return true;*/
                return true;
            }, (p) =>
       {
           WorkRole = (WorkRoleIndex == 1) ? "Staff" : "Manager";
           Gender = (GenderIndex == 1) ? "Male" : (GenderIndex == 2) ? "Female" : "Other";
           using (var db = new DeCoDbContext())
           {
               var query = from b in db.Employees select b;
               var employee = query.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
               employee.DisplayName = DisplayName;
               employee.WorkRole = WorkRole;
               employee.PhoneNumber = PhoneNumber;
               employee.Email = Email;
               employee.Gender = Gender;
               employee.ContractDate = ContractDate;
               if (Username != "" && Username != null)
               {
                   var query1 = from b in db.Users where (b.Username == Username && b.EmployeeId == null) select b;
                   if (query1.Count() > 0)//username existed and empId null
                   {
                       var user = from b in db.Users where (b.Username == Username) select b;
                       user.First().EmployeeId = employee.Id;
                   }
                   else
                   {
                       int y = db.Users.Count();
                       var user = new User()
                       {
                           Id = 9000 + y + 1,
                           Username = Username,
                           PasswordHash = Password,
                           EmployeeId = employee.Id,
                       };
                       db.Users.Add(user);
                   }
               }
               db.SaveChanges();
           }
           SelectedItem.DisplayName = DisplayName;
           SelectedItem.WorkRole = WorkRole;
           SelectedItem.PhoneNumber = PhoneNumber;
           SelectedItem.Email = Email;
           SelectedItem.Gender = Gender;
           SelectedItem.ContractDate = ContractDate;
           WorkRoleIndex = (SelectedItem.WorkRole == "Staff") ? 1 : 2;
           GenderIndex = (SelectedItem.Gender == "Male") ? 1 : (SelectedItem.Gender == "Female") ? 2 : 3;
           loadList();
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
                    var query = from b in db.Employees select b;
                    var employee = query.Where(x => x.Id == SelectedItem.Id).FirstOrDefault();
                    employee.Deleted = true;

                    var query1 = from b in db.Users select b;
                    var users = query1.Where(x => x.EmployeeId == SelectedItem.Id).FirstOrDefault();
                    if (users != null)
                        users.EmployeeId = null;

                    db.SaveChanges();

                }
                employees.Remove(SelectedItem);
                ClearFields();
            });
            cancelCommand = new RelayCommand<Object>((p) => { return true; }, (p) => { ClearFields(); });
            string tempUsername = "* * * * * Change Password * * * * *";
            changePasswordCommand = new RelayCommand<object>((p) =>
            {
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(DisplayName) || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(Email) || GenderIndex == 0 || WorkRoleIndex == 0)
                {
                    return false;
                }
                if (SelectedItem == null)
                {
                    return false;
                }
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Users where (b.Username == Username && b.EmployeeId != null) select b;
                    {
                        if (query.Count() > 0)
                            return true;
                    }
                    if (Username == "* * * * * Change Password * * * * *")
                    {
                        return true;
                    }
                }
                return false;
            }, (p) =>
            {
                if (Username == "* * * * * Change Password * * * * *")
                {
                    using (var db = new DeCoDbContext())
                    {
                        var query = from b in db.Users select b;
                        var user = query.Where(x => x.EmployeeId == SelectedItem.Id).FirstOrDefault();
                        user.PasswordHash = Password;
                        db.SaveChanges();
                        Username = tempUsername;
                        Password = user.PasswordHash;
                    }
                }
                else
                {
                    tempUsername = Username;
                    Username = "* * * * * Change Password * * * * *";

                }
            });
            disconnectCommand = new RelayCommand<object>((p) =>
            {
                if (SelectedItem == null)
                {
                    return false;
                }
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Users where (b.EmployeeId == SelectedItem.Id) select b;
                    if (query.Count() > 0) return true;
                    else return false;
                }
            }, (p) =>
            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Users select b;
                    var users = query.Where(x => x.EmployeeId == SelectedItem.Id).FirstOrDefault();
                    users.EmployeeId = null;
                    db.SaveChanges();
                    Username = Password = null;
                }
            });
            Searching = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>

            {
                using (var db = new DeCoDbContext())
                {
                    var query = from b in db.Employees select b;

                    if (string.IsNullOrEmpty(SearchString))
                    {
                        query = query.Where(x => x.Deleted != true);
                    }
                    else
                    {
                        query = query.Where(x => (x.DisplayName.Trim()).Contains((SearchString.Trim())) && x.Deleted != true);
                    }
                    ObservableCollection<Employee> list = new ObservableCollection<Employee>(query.ToList());
                    employees = list;
                }

            });
        }


        private void ClearFields()
        {
            DisplayName = PhoneNumber = Email = WorkRole = Username = Password = null;
            GenderIndex = WorkRoleIndex = 0;
            ContractDate = DateTime.Now;
            loadList();
            SelectedItem = null;
        }
        void loadList()
        {

            using (DeCoDbContext dbContext = new DeCoDbContext())
            {
                var query = from b in dbContext.Employees where (b.Deleted != true) select b;
                ObservableCollection<Employee> list = new ObservableCollection<Employee>(query.ToList());
                employees = list;
            }

        }
    }
}
