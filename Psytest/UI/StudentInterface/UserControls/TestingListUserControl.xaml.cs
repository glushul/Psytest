using Psytest.Data;
using Psytest.UI.Windows;
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

namespace Psytest.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TestingListUserControl.xaml
    /// </summary>
    public partial class TestingListUserControl : UserControl
    {

        public TestingListUserControl(Testing testing)
        {
            InitializeComponent();
            DataContext = testing;
            TextBlockName.Text = testing.Id + ". " + testing.Name;
        }

        private void ButtonTakeTest_Click(object sender, RoutedEventArgs e)
        {
            StudentInfoWindow studentWindow = new StudentInfoWindow(DataContext as Testing);
            studentWindow.ShowDialog();
        }
    }
}
