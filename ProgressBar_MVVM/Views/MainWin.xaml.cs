using ProgressBar_MVVM.Utils;
using ProgressBar_MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ProgressBar_MVVM
{
    /// <summary>
    /// Interaction logic for MainWin.xaml
    /// </summary>
    public partial class MainWin : Window
    {
        public MainWin()
        {
            InitializeComponent();
            
            MainWinVM mainWinVM = new MainWinVM();
            DataContext = mainWinVM;
        }

        private void Start_Btn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Btn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
