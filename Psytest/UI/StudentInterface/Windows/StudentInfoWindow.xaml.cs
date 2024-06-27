using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using static Psytest.Instruments.Manager;

namespace Psytest.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для StudentInfoWindow.xaml
    /// </summary>
    public partial class StudentInfoWindow : Window
    {
        Testing _testing;
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
                    //Удаление прошлых результатов, если студент уже проходил тестирование
                    List<StudentResult> studentResultsToDelete = new List<StudentResult> { };

                    string thisSemester= Manager.GetSemester(DateTime.Now);
                    var studentResults = PsytestDBEntities.GetContext().StudentResults.
                        Where(p => p.GroupId == student.Group.Id && p.TestingId == _testing.Id && p.TestingSemester == thisSemester &&
                        p.Age == student.Age && p.GenderId == student.Gender.Id).ToList();

                    foreach (var studentResult in studentResults)
                    {
                        if (Crypter.Decrypt(studentResult.Surname) == student.Surname && Crypter.Decrypt(studentResult.Name) == student.Name)
                            studentResultsToDelete.Add(studentResult);
                    }

                    if (studentResultsToDelete.Count == 0)
                    {
                        FrameNavigation.Navigate(new TestingPage(student, _testing));
                        this.Close();
                    }
                    else
                    {
                        PsytestDBEntities.GetContext().StudentResults.RemoveRange(studentResultsToDelete);
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
        private void TextBoxAge_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonStart_Click(this, new RoutedEventArgs());
            }
        }

        private void TextBoxName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonStart_Click(this, new RoutedEventArgs());
            }
        }

        private void TextBoxSurname_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonStart_Click(this, new RoutedEventArgs());
            }
        }

        private void ComboBoxGroup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonStart_Click(this, new RoutedEventArgs());
            }
        }

        private void ComboBoxGender_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonStart_Click(this, new RoutedEventArgs());
            }
        }
    }
}
