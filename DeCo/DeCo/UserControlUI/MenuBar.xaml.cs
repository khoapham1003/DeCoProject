using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DeCo.ViewModel;
namespace DeCo.UserControlUI
{
    /// <summary>
    /// Interaction logic for MenuBar.xaml
    /// </summary>
    public partial class MenuBar : UserControl
    {
        public MenuBarViewModel Viewmodel { get; set; }
        public MenuBar()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new MenuBarViewModel();
        }
    }
}
