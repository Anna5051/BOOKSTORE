using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = AppConnect.BookstoreModel.Users
                .FirstOrDefault(u => u.Login == tbLogin.Text && u.Password == tbPassword.Password);

            if (user != null)
            {
                App.CurrentUser = user;

                MessageBox.Show($"Здравствуйте, {user.FullName}!");
                AppData.MainFrame.FrameMain.Navigate(new PageProducts());
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new Registration());
        }
    }
}
