using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DeCo.Model;

namespace DeCo
{
    public class Const : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
        public static void OnStaticPropertyChanged([CallerMemberName] string propertyName = null)
        {
            StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }
        private static readonly Const instance = new Const();
        public static Const Instance
        {
            get
            {
                return instance;
            }
        }
        private Const() { }
        static private User _ActiveUser;
        static public User ActiveUser
        {
            get
            {
                using (var db = new DeCoDbContext())
                {
                    if (_ActiveUser != null)
                    {
                        var query = from b in db.Employees where (b.Id == _ActiveUser.EmployeeId) select b;
                        _ActiveUser.Employee = query.FirstOrDefault();
                    }
                }
                return _ActiveUser;
            }
            set { _ActiveUser = value; OnStaticPropertyChanged(); }
        }
        static private bool _IsManager;
        static public bool IsManager { get => _IsManager; set { _IsManager = value; OnStaticPropertyChanged(); } }
        static public Visibility _ManagerVisibility = Visibility.Visible;
        static public Visibility ManagerVisibility
        {
            get => _ManagerVisibility;
            set
            {
                if (_ManagerVisibility != value)
                {
                    _ManagerVisibility = value;
                    OnStaticPropertyChanged("ManagerVisibility");
                }
            }
        }
    }
}
