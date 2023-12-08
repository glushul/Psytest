using Psytest.Data;
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

namespace Psytest.UI.PsychologistInterface.UserControls
{
    /// <summary>
    /// Логика взаимодействия для YearTabUserControl.xaml
    /// </summary>
    public partial class YearTabUserControl : UserControl
    {
        public YearTabUserControl(Testing testing, Group group, int year)
        {
            InitializeComponent();
            TextBlockFullName.Text = testing.Id.ToString() + ". " + testing.FullName.ToString();
            TextBlockStudentCount.Text = "Количество прошедших: " + PsytestDBEntities.GetContext().StudentResults.
                Where(p => p.GroupId == group.Id && p.TestingId == testing.Id && p.TestingYear == year).
                GroupBy(p => new { p.Surname, p.Name }).Count(); ;
        }
    }
}
