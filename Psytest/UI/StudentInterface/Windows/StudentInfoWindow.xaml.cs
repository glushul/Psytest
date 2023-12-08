using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
using static Psytest.Instruments.Manager;

namespace Psytest.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для StudentInfoWindow.xaml
    /// </summary>
    public partial class StudentInfoWindow : Window
    {
        Testing _testing = new Testing();
        public StudentInfoWindow(Testing chosenTesting)
        {
            InitializeComponent();
            _testing = chosenTesting;

            ComboBoxGroup.ItemsSource = PsytestDBEntities.GetContext().Groups.ToList().OrderBy(p => p.Number);

            ComboBoxGender.ItemsSource = PsytestDBEntities.GetContext().Genders.ToList();
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            int result;
            Int32.TryParse(TextBoxAge.Text, out result);
            Student student = new Student( TextBoxName.Text.Replace(" ", ""),
                TextBoxSurname.Text.Replace(" ", ""),
                result, ComboBoxGroup.SelectedItem as Group,
                ComboBoxGender.SelectedItem as Gender);

            if (student.Name != "" && student.Surname != "" && student.Age > 0 && student.Group != null && student.Gender != null)
            {
                if (MessageBox.Show("Вы уверены, что хотите начать тестирование? " +
                    "После нажатия кнопки Да начнется тестирование без возможности выйти из него.",
                    "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {
                    var studentResults = PsytestDBEntities.GetContext().StudentResults.
                    Where(p => p.Name == student.Name 
                    && p.Surname == p.Surname && p.Age == student.Age 
                    && p.GroupId == student.Group.Id && p.GenderId == student.Gender.Id
                    && p.TestingId == _testing.Id && p.TestingYear == DateTime.Now.Year).ToList();
                    if (studentResults.Count == 0)
                    {
                        FrameNavigation.Navigate(new TestingPage(student, _testing));
                        this.Close();
                    }
                    else
                    {
                        PsytestDBEntities.GetContext().StudentResults.RemoveRange(studentResults);
                        PsytestDBEntities.GetContext().SaveChanges();
                        FrameNavigation.Navigate(new TestingPage(student, _testing));
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Заполните все данные", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
