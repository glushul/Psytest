using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.PsychologistInterface.Pages;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace Psytest.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddEditGroupWindow.xaml
    /// </summary>
    public partial class AddGroupWindow : Window
    {
        Group _group = new Group();
        public AddGroupWindow()
        {
            InitializeComponent();

            //Заполненик комбо боксов
            ComboBoxForm.Items.Add("Бюджет");
            ComboBoxForm.Items.Add("Коммерция");
            ComboBoxForm.SelectedIndex = 0;
            ComboBoxDepartments.ItemsSource = PsytestDBEntities.
                GetContext().Faculties.ToList();
            ComboBoxDepartments.SelectedIndex = 0;

            DataContext = _group;
        }
        
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            //Составление полного наименования группы
            string form = "";
            if (ComboBoxForm.SelectedIndex == 0)
                form = "Д9";
            else
                form = "КД9";
            _group.FullName = _group.Number.ToString() + "-"
                + form + "-" + (ComboBoxDepartments.SelectedValue as Faculty).Name;
            _group.FacultyId = (ComboBoxDepartments.SelectedValue as Faculty).Id;


            //Обнаружение ошибок
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

        //Обработчики кнопок окна
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Обработчики нажатия кнопки Enter
        private void TextBoxNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonSave_Click(this, new RoutedEventArgs());
            }
        }

        private void ComboBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonSave_Click(this, new RoutedEventArgs());
            }
        }

        private void ComboBoxDepartments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonSave_Click(this, new RoutedEventArgs());
            }
        }
    }
}
