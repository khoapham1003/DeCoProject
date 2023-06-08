using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;

using DeCo.Model;

namespace DeCo.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        public bool IsLogin { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        private string _Username;
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(); } }
        private string _Password;
        public string Password { get { return _Password; } set { _Password = value; OnPropertyChanged(); } }
        private string _loginMsg;
        public string loginMsg { get { return _loginMsg; } set { _loginMsg = value; OnPropertyChanged(); } }
        public LoginViewModel()
        {
            IsLogin = false;
            Username = Password = "";
            LoginCommand= new RelayCommand<Window>((p) => {return true;}, (p)=>{Login(p);});
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) => { Password = p.Password; });
        }
        void Login(Window p)
        {
            loginMsg = "";
            if (p == null)
                return;

            using (var db = new DeCoDbContext())
            {
                var query = from b in db.Users where (b.Username == Username && b.PasswordHash == Password) select b;
                if (query.Count() > 0)
                {
                    Const.ActiveUser = query.FirstOrDefault();
                    OnPropertyChanged("Const.ActiveUser");
                    IsLogin = true;
                    p.Close();
                }
                else
                {
                    loginMsg = "Invalid Username or Password!Please try again...";
                    IsLogin = false;
                }
            }
        }
    }
}
