using Psytest.Data;
using Psytest.UI.UserControls;
using System.Linq;
using System.Windows.Controls;

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

            //Добавление информации о тестированиях на страницу
            var testings = PsytestDBEntities.GetContext().Testings.ToList();
            foreach (var testing in testings)
                WrapPanelTesting.Children.Add(new TestingListUserControl(testing));
        }
    }
}
