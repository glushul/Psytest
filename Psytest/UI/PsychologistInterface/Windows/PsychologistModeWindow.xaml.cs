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
using System.Windows.Shapes;

namespace Psytest.UI.Windows
{
    /// <summary>
    /// Логика взаимодействия для PsychologistModeWindow.xaml
    /// </summary>
    public partial class PsychologistModeWindow : Window
    {
        public PsychologistModeWindow()
        {
            InitializeComponent();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordBoxCode.Password == "5009265")
            {
                Manager.FrameNavigation.Navigate(new GroupListPage());
                this.Close();
            }
            else
            {
                MessageBox.Show("Неправильно введен код", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                PasswordBoxCode.Password = "";
            }
        }
    }
}
