using Psytest.Instruments;
using Psytest.UI.Pages;
using Psytest.UI.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Psytest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); 
            Manager.FrameNavigation = FrameNavigation;
            FrameNavigation.Navigate(new TestingListPage());
        }

        /// <summary>
        /// Переход на страницу назад
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameNavigation.GoBack();
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {

            if (Manager.FrameNavigation.Content.GetType() == typeof(TestingListPage))
            {
                PsychologistModeWindow psychologistModeWindow = new PsychologistModeWindow();
                psychologistModeWindow.ShowDialog();
            }
            else
            {
                Manager.FrameNavigation.Navigate(new TestingListPage());
            }
        }

        private void FrameNavigation_Navigated(object sender, NavigationEventArgs e)
        {
            if (Manager.FrameNavigation.Content.GetType() == typeof(TestingListPage))
            {
                TextBlockNavigation.Text = "Тестирования";
                ButtonExit.Visibility = Visibility.Visible;
                ButtonBack.Visibility = Visibility.Hidden;
            }
            else if (Manager.FrameNavigation.Content.GetType() == typeof(GroupPage))
            {
                TextBlockNavigation.Text = "Группа";
                ButtonBack.Visibility = Visibility.Visible;
                ButtonExit.Visibility = Visibility.Visible;
            }
            else if (Manager.FrameNavigation.Content.GetType() == typeof(GroupListPage))
            {
                TextBlockNavigation.Text = "Группы";
                ButtonBack.Visibility = Visibility.Hidden;
                ButtonExit.Visibility = Visibility.Visible;
            }
            else if (Manager.FrameNavigation.Content.GetType() == typeof(TestingPage))
            {
                TextBlockNavigation.Text = "Тестирование";
                ButtonExit.Visibility = Visibility.Hidden;
                ButtonBack.Visibility = Visibility.Hidden;
            }
        }
    }
}
