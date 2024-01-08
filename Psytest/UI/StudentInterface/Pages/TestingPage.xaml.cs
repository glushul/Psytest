using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.UserControls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static Psytest.Instruments.Manager;

namespace Psytest.UI.Pages
{
    /// <summary>
    /// Логика взаимодействия для TestingPage.xaml
    /// </summary>
    public partial class TestingPage : Page
    {
        Testing _testing;
        Student _student; 

        public TestingPage(Student student, Testing testing)
        {
            InitializeComponent();

            //Очищение словарей
            PointCounter.questionAnswerPairs.Clear();
            PointCounter.categoryPointCountPairs.Clear();

            _student = student;
            _testing = testing;
            Manager.NavigatingText = _testing.Name;

            //Добавление вопросов и вариантов ответов на страницу
            var questions = PsytestDBEntities.GetContext().Questions.
                Where(p => p.TestingId == _testing.Id).ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                PointCounter.questionAnswerPairs.Add(i + 1, 0);
                MainStackPanel.Children.Add(new TestingUserControl(i + 1, questions[i]));
            }

            var categories = PsytestDBEntities.GetContext().Categories.Where(p => p.TestingId == _testing.Id).ToList();
            foreach (var category in categories)
                PointCounter.categoryPointCountPairs.Add(category.Id, 0);
        }

        private void ButtonEnd_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите завершить тестирование " +
                "и сохранить результат?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                foreach (var questionAnswerPair in PointCounter.questionAnswerPairs)
                {
                    if (questionAnswerPair.Value == 0)
                    {
                        MessageBox.Show($"Ответ на вопрос №{questionAnswerPair.Key} не выбран");
                        return;
                    }
                    else
                    {
                        var points = PsytestDBEntities.GetContext().Points.Where(p => p.AnswerId == questionAnswerPair.Value).ToList();
                        foreach (var point in points)
                            PointCounter.categoryPointCountPairs[point.CategoryId] += point.PointSum;
                    }
                }

                //Сохранение результатов в базу данных
                foreach (var categoryPointCountPair in PointCounter.categoryPointCountPairs)
                {
                    StudentResult studentResult = new StudentResult();
                    studentResult.TestingId = _testing.Id;
                    studentResult.Surname = _student.Surname;
                    studentResult.Name = _student.Name;
                    studentResult.Gender = _student.Gender;
                    studentResult.Group = _student.Group;
                    studentResult.Age = _student.Age;
                    studentResult.TestingYear = DateTime.Now.Year;
                    studentResult.PointSum = categoryPointCountPair.Value;
                    studentResult.CategoryId = categoryPointCountPair.Key;
                    try
                    {
                        PsytestDBEntities.GetContext().StudentResults.Add(studentResult);
                        PsytestDBEntities.GetContext().SaveChanges();
                        FrameNavigation.Navigate(new TestingListPage());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
    }
}
