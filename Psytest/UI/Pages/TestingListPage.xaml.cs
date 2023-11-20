using Psytest.Data;
using Psytest.UI.UserControls;
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

namespace Psytest.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для TestingListPage.xaml
    /// </summary>
    public partial class TestingListPage : Page
    {
        public TestingListPage()
        {
            InitializeComponent();
            var testings = PsytestDBEntities.GetContext().Testings.ToList();
            foreach (var testing in testings)
                WrapPanelTesting.Children.Add(new TestingListUserControl() { DataContext = testing });
        }
    }
}
