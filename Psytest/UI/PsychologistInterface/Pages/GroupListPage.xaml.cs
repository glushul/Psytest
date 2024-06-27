using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.Windows;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Psytest.UI.PsychologistInterface.Pages
{
    /// <summary>
    /// Логика взаимодействия для GroupListPage.xaml
    /// </summary>
    public partial class GroupListPage : Page
    {
        public GroupListPage()
        {
            InitializeComponent();

            UpdateGroups();

            ComboBoxFaculties.Items.Add(new Faculty() { Name = "Все" });
            foreach(var faculty in PsytestDBEntities.GetContext().Faculties.ToList())
                ComboBoxFaculties.Items.Add(faculty);
            ComboBoxFaculties.SelectedIndex = 0;
        }

        private void ComboBoxCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        private void ComboBoxFaculties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        private void TextBoxNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGroups();
        }

        private void UpdateGroups()
        {
            //Сортировка и фильтр групп
            var groups = PsytestDBEntities.GetContext().Groups.ToList();

            groups = groups.Where(p => p.Number.ToString().Contains(TextBoxNumber.Text)).
                OrderBy(p => p.Number).ToList();

            if (ComboBoxFaculties.SelectedIndex == 0)
                groups = groups.ToList();
            else
                groups = groups.Where(p => p.FacultyId == ComboBoxFaculties.SelectedIndex).ToList();

            if (groups.Count() > 0)
            {
                GroupListView.Visibility = Visibility.Visible;
                TextBlockNoResults.Visibility = Visibility.Hidden;
                GroupListView.ItemsSource = groups;
            }
            else
            {
                GroupListView.Visibility = Visibility.Hidden;
                TextBlockNoResults.Visibility = Visibility.Visible;
            }
        }

        private void ButtonDeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (GroupListView.SelectedItems.Count > 0)
            {
                var groupsForDeleting = GroupListView.SelectedItems.Cast<Group>().ToList();
                if (MessageBox.Show($"Вы точно хотите удалить {groupsForDeleting.Count()} объект", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        PsytestDBEntities.GetContext().Groups.RemoveRange(groupsForDeleting);
                        PsytestDBEntities.GetContext().SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        Manager.FrameNavigation.Navigate(new GroupListPage());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
            else
                MessageBox.Show("Выберите элементы для удаления", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ButtonAddGroup_Click(object sender, RoutedEventArgs e)
        {
            AddGroupWindow addEditGroupWindow = new AddGroupWindow();
            addEditGroupWindow.ShowDialog();
        }
    }
}
