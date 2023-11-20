using Psytest.Data;
using Psytest.Instruments;
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

namespace Psytest.UI.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TestingUserControl.xaml
    /// </summary>
    public partial class TestingUserControl : UserControl
    {
        public Answer chosenAnswer { get; set; }
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
