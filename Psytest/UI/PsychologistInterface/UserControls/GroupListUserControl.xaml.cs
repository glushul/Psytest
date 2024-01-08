using Psytest.Data;
using Psytest.Instruments;
using Psytest.UI.PsychologistInterface.Pages;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.FrameNavigation.Navigate(new GroupPage(DataContext as Group));
        }
    }
}
