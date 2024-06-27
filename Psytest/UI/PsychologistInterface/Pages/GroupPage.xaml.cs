using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.PsychologistInterface.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Psytest.UI.PsychologistInterface.Pages
{
    /// <summary>
    /// Логика взаимодействия для GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        Group _group;
        Dictionary<string, StackPanel> yearTabContentPairs = new Dictionary<string, StackPanel>();
        public GroupPage(Group group)
        {
            InitializeComponent();
            yearTabContentPairs.Clear();
            _group = group;
            Manager.NavigatingText = group.FullName;

            var semesters = PsytestDBEntities.GetContext().StudentResults.
                Where(p => p.GroupId == group.Id).Select(p => p.TestingSemester).Distinct().ToList();
            if (semesters.Count() == 0)
            {
                TextBlockNoResults.Visibility = Visibility.Visible;
                YearTabControl.Visibility = Visibility.Hidden;
            }
            else
            {
                TextBlockNoResults.Visibility = Visibility.Hidden;
                YearTabControl.Visibility = Visibility.Visible;

                foreach (var semester in semesters)
                {
                    TabItem tabItem = new TabItem()
                    {
                        Header = semester,
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
                        GroupTestingTabUserControl yearTabUserControl = new GroupTestingTabUserControl(testing, _group, tabItem.Header.ToString());
                        stackPanel.Children.Add(yearTabUserControl);
                    }
                    yearTabContentPairs.Add(semester, stackPanel);
                }
            }
        }

        private void YearTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabItem tabItem = YearTabControl.SelectedItem as TabItem;
            tabItem.Content = yearTabContentPairs[tabItem.Header.ToString()];
        }
    }
}
