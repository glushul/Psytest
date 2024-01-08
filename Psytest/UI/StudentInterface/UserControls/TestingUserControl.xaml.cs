using Psytest.Data;
using Psytest.Instruments;
using System.Linq;
using System.Windows.Controls;

namespace Psytest.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TestingUserControl.xaml
    /// </summary>
    public partial class TestingUserControl : UserControl
    {
        public TestingUserControl(int numerator, Question question)
        {
            InitializeComponent();

            DataContext = question;

            TextBlockNumerator.Text = numerator.ToString() + ".";

            var answers = PsytestDBEntities.GetContext().Answers.Where(p => p.QuestionId == question.Id).ToList();
            foreach(var answer in answers)
            {
                RadioButton radioButton = new RadioButton()
                {
                    DataContext = answer,
                    Content = answer.Description,
                    FontSize = 20,  
                    GroupName = "Question" + numerator.ToString()
                };
                radioButton.Checked += (s, e) =>
                {
                    PointCounter.questionAnswerPairs[numerator] = (radioButton.DataContext as Answer).Id;
                };
                MainStackPanel.Children.Add(radioButton);
            }
            
        }
    }
}
