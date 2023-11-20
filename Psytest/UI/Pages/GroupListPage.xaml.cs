using Psytest.Data;
using Psytest.Instruments;
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

namespace Psytest.UI.Pages
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

            ComboBoxCourse.Items.Add("Все");
            List<int> courses = new List<int>() { 1, 2, 3, 4};
            ComboBoxCourse.Items.Add(courses);
            ComboBoxCourse.SelectedIndex = 0;

            ComboBoxFaculties.Items.Add(new Faculty() { Name = "Все" });
            var faculties = PsytestDBEntities.GetContext().Faculties.ToList();
            ComboBoxFaculties.Items.Add(faculties);
            ComboBoxFaculties.SelectedIndex = 0;
        }

        /// <summary>
        /// Фильтрация по курсу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// Фильтрация по отделениям
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxFaculties_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// Поиск по номеру группы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateGroups();
        }

        /// <summary>
        /// Именение интерфейса при фильтрации и поиске
        /// </summary>
        private void UpdateGroups()
        {
            var groups = PsytestDBEntities.GetContext().Groups.ToList();

            groups = groups.Where(p => p.Number.ToString().Contains(TextBoxNumber.Text)).
                OrderBy(p => p.Number).ToList();

            if (ComboBoxCourse.SelectedIndex == 0)
                groups = groups.ToList();
            else
                groups = groups.Where(p => p.Course == ComboBoxCourse.SelectedIndex).ToList();

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

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Добавление группы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddGroup_Click(object sender, RoutedEventArgs e)
        {
            AddEditGroupWindow addEditGroupWindow = new AddEditGroupWindow();
            addEditGroupWindow.ShowDialog();
        }
    }
}
