using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.Pages;
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
using System.Windows.Shapes;

namespace Psytest.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditGroupWindow.xaml
    /// </summary>
    public partial class AddEditGroupWindow : Window
    {
        Group _group = new Group();
        public AddEditGroupWindow()
        {
            InitializeComponent();
            //Заполненик комбо боксов
            ComboBoxForm.Items.Add("Бюджет");
            ComboBoxForm.Items.Add("Коммерция");
            ComboBoxForm.SelectedIndex = 0;
            ComboBoxCourse.Items.Add(1);
            ComboBoxCourse.Items.Add(2);
            ComboBoxCourse.Items.Add(3);
            ComboBoxCourse.Items.Add(4);
            ComboBoxCourse.SelectedIndex = 0;
            ComboBoxDepartments.ItemsSource = PsytestDBEntities.
                GetContext().Faculties.ToList();
            ComboBoxDepartments.SelectedIndex = 0;
            DataContext = _group;
        }

        /// <summary>
        /// Сохранение группы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            string form = "";
            if (ComboBoxForm.SelectedIndex == 0)
                form = "Д9";
            else
                form = "КД9";
            _group.FullName = _group.Number.ToString() + "-"
                + form + "-" + (ComboBoxCourse.SelectedIndex + 1).ToString()
                + (ComboBoxDepartments.SelectedValue as Faculty).Name;
            _group.Course = ComboBoxCourse.SelectedIndex + 1;
            _group.FacultyId = (ComboBoxDepartments.SelectedValue as Faculty).Id;

            StringBuilder errors = new StringBuilder();

            if (_group.Number <= 0)
                errors.AppendLine("Укажите корректный номер");
            if (string.IsNullOrEmpty(_group.FullName))
                errors.AppendLine("Укажите имя");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_group.Id == 0)
                PsytestDBEntities.GetContext().Groups.Add(_group);

            try
            {
                PsytestDBEntities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены!");
                Manager.FrameNavigation.Navigate(new GroupListPage());
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
