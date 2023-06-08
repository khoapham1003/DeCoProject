using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DeCo.Model;
using System.Windows.Input;
using DeCo.View;

namespace DeCo.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public bool Isloaded = false;
        public ICommand LoadedWindowCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Isloaded = true;
                if (p == null) return;
                p.Hide();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.ShowDialog();
                if (loginWindow.DataContext == null) return;
                var loginVM = loginWindow.DataContext as LoginViewModel;
                if (loginVM.IsLogin)
                {
                    Permission();
                    p.Show();
                }
                else p.Close();
            });
            LogOutCommand = new RelayCommand<Window>((p) => true, (p) => { Logout(p); });
        }
        public void Permission()
        {
            using (var db = new DeCoDbContext())
            {
                var query = from b in db.Employees where (b.Id == Const.ActiveUser.EmployeeId) select b;
                if (query.Count() > 0)
                {
                    var ActiveEmployee = query.FirstOrDefault();
                    if (ActiveEmployee.WorkRole == "Manager")
                    {
                        MessageBox.Show("Have a great day!\n" + Const.ActiveUser.Employee.DisplayName + " (-Manager-)", "Welcome", MessageBoxButton.OK);
                        Const.IsManager = true;
                        Const.ManagerVisibility = Visibility.Visible;
                    }
                    if (ActiveEmployee.WorkRole == "Staff")
                    {
                        MessageBox.Show("Have a great day!\n" + Const.ActiveUser.Employee.DisplayName + " (-Staff-)", "Welcome", MessageBoxButton.OK);
                        Const.IsManager = false;
                        Const.ManagerVisibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    if (Const.ActiveUser.Username=="admin")
                    {
                        Const.IsManager = true;
                        Const.ManagerVisibility = Visibility.Visible;
                        MessageBox.Show("Hello admin, Have a good day!", "Welcome", MessageBoxButton.OK, MessageBoxImage.Question);
                    }
                    else
                    {
                        Const.IsManager = false;
                        Const.ManagerVisibility = Visibility.Collapsed;
                        MessageBox.Show("Your account isn't connected", "Welcome", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }
        void Logout(Window p)
        {
            LoginWindow loginWindow = new LoginWindow();
            var loginVM = loginWindow.DataContext as LoginViewModel;
            loginVM.IsLogin = false;
            Isloaded = true;
            if (p == null) return;
            p.Hide();
            loginWindow.ShowDialog();
            if (loginWindow.DataContext == null) return;
            if (loginVM.IsLogin)
            {
                Permission();
                p.Show();
            }
            else p.Close();
        }

    }
}

