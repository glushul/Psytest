using Psytest.Instruments;
using Psytest.UI.PsychologistInterface.Pages;
using System.Windows;
using System.Windows.Input;

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

        //Обработчики кнопок окна
        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Обработчики нажатия кнопки Enter
        private void PasswordBoxCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonEnter_Click(this, new RoutedEventArgs());
            }
        }
    }
}
