using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.PsychologistInterface.UserControls;
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

namespace Psytest.UI.PsychologistInterface.Pages
{
    /// <summary>
    /// Логика взаимодействия для GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        Group _group;
        Dictionary<int, StackPanel> yearTabContentPairs = new Dictionary<int, StackPanel>();
        public GroupPage(Group group)
        {
            InitializeComponent();
            yearTabContentPairs.Clear();
            _group = group;
            Manager.NavigatingText = group.FullName;

            var years = PsytestDBEntities.GetContext().StudentResults.
                Where(p => p.GroupId == group.Id).
                GroupBy(p => p.TestingYear).ToArray().OrderByDescending(p => p.Key);
            if (years.Count() == 0)
            {
                TextBlockNoResults.Visibility = Visibility.Visible;
                YearTabControl.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBlockNoResults.Visibility = Visibility.Hidden;
                YearTabControl.Visibility = Visibility.Visible;

                foreach (var year in years)
                {
                    TabItem tabItem = new TabItem()
                    {
                        Header = year.Key,
                        FontSize = 22
                    };
                    YearTabControl.Items.Add(tabItem);

                    StackPanel stackPanel = new StackPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Center
                    };
                    var testings = PsytestDBEntities.GetContext().Testings.ToList();
                    foreach (var testing in testings)
                    {
                        GroupTestingTabUserControl yearTabUserControl = new GroupTestingTabUserControl(testing, _group, Int32.Parse(tabItem.Header.ToString()));
                        stackPanel.Children.Add(yearTabUserControl);
                    }
                    yearTabContentPairs.Add(year.Key, stackPanel);
                }
            }
        }

        private void YearTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tabItem = YearTabControl.SelectedItem as TabItem;
            tabItem.Content = yearTabContentPairs[Int32.Parse(tabItem.Header.ToString())];
        }
    }
}
