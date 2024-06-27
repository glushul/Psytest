using Psytest.Instruments;
using Psytest.UI.Pages;
using Psytest.UI.PsychologistInterface.Pages;
using Psytest.UI.Windows;
using Rijndael256;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Psytest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class NavigatingWindow : Window
    {
        public NavigatingWindow()
        {
            InitializeComponent();

            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.WindowState = WindowState.Maximized;

            Manager.FrameNavigation = FrameNavigation;
            FrameNavigation.Navigate(new TestingListPage());
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameNavigation.GoBack();
        }

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
            //Изменение видимости кнопок и текста главного заголовка окна
            if (Manager.FrameNavigation.Content.GetType() == typeof(TestingListPage))
            {
                TextBlockNavigation.Text = "Тестирования";
                ButtonExit.Visibility = Visibility.Visible;
                ButtonBack.Visibility = Visibility.Hidden;
            }
            else if (Manager.FrameNavigation.Content.GetType() == typeof(GroupPage))
            {
                TextBlockNavigation.Text = "Группа " + Manager.NavigatingText;
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
                TextBlockNavigation.Text = Manager.NavigatingText;
                ButtonExit.Visibility = Visibility.Hidden;
                ButtonBack.Visibility = Visibility.Hidden;
            }
        }

        //Обработчики кнопок окна
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonMaximise_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void ButtonCollapse_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
