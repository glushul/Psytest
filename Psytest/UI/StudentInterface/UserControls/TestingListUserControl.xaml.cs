using Psytest.Data;
using Psytest.UI.Windows;
using System.Windows;
using System.Windows.Controls;

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
