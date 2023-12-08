using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.Pages;
using Psytest.UI.PsychologistInterface.Pages;
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
    /// Логика взаимодействия для GroupListUserControl.xaml
    /// </summary>
    public partial class GroupListUserControl : UserControl
    {
        public GroupListUserControl()
        {
            InitializeComponent();
        }

        private void ButtonSeeGroup_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameNavigation.Navigate(new GroupPage(DataContext as Group));
        }
    }
}
