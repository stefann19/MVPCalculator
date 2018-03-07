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

namespace MVPCalculator
{
    /// <summary>
    /// Interaction logic for CButton.xaml
    /// </summary>
    public partial class CButton : UserControl
    {
        public CButton()
        {
            InitializeComponent();
            Button.MouseDown += Button_MouseDown;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
