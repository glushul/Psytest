using Psytest.Data;
using Psytest.Instruments;
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

            _student = student;
            _testing = testing;

            var questions = PsytestDBEntities.GetContext().Questions.
                Where(p => p.TestingId == _testing.Id).ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                // You can use questions[i].Id here
                PointCounter.questionAnswerPairs.Add(i + 1, 0);
                MainStackPanel.Children.Add(new TestingUserControl(i + 1, questions[i]));
            }

            var categories = PsytestDBEntities.GetContext().Categories.Where(p => p.TestingId == _testing.Id).ToList();
            foreach (var category in categories)
            {
                PointCounter.categoryPointCountPairs.Add(category.Id, 0);
            }
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
                        MessageBox.Show($"Ответ на {questionAnswerPair.Key} вопрос не дан");
                        return;
                    }
                    else
                    {
                        var points = PsytestDBEntities.GetContext().Points.Where(p => p.AnswerId == questionAnswerPair.Value).ToList();
                        foreach (var point in PsytestDBEntities.GetContext().Points.Where(p => p.AnswerId == questionAnswerPair.Value).ToList())
                        {
                            // what happens if this key doesn't exist? better do if(PointCounter.categoryPointCountPairs.contains(point.CategoryId)) { logic } 
                            PointCounter.categoryPointCountPairs[point.CategoryId] += point.PointSum;
                        }
                    }
                }

                foreach (var categoryPointCountPair in PointCounter.categoryPointCountPairs)
                {
                    StudentResult studentResult = new StudentResult();
                    studentResult.TestingId = _testing.Id;
                    studentResult.Surname = _student.Surname;
                    studentResult.Name = _student.Name;
                    studentResult.Gender = _student.Gender;
                    studentResult.Group = _student.Group;
                    studentResult.Age = _student.Age;
                    studentResult.PointSum = categoryPointCountPair.Value;
                    studentResult.CategoryId = categoryPointCountPair.Key;
                    PsytestDBEntities.GetContext().StudentResults.Add(studentResult);
                    PsytestDBEntities.GetContext().SaveChanges();
                    FrameNavigation.Navigate(new TestingListPage());
                }
                PointCounter.questionAnswerPairs.Clear();
                PointCounter.categoryPointCountPairs.Clear();
            }
        }
    }
}
