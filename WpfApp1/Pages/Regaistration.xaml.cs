using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class Registration : Page
    {
        private Users _currentUser = new Users();

        public Registration()
        {
            InitializeComponent();
            DataContext = _currentUser;
        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            string name = nameTb.Text.Trim();
            string login = loginTb.Text.Trim();
            string password = passwordTb.Password.Trim();
            string repeatPassword = repeatPasswordTb.Password.Trim();
            string email = emailTb.Text.Trim();
            DateTime? birthDate = birthDateTb.SelectedDate;
            string phone = phoneTb.Text.Trim();

            StringBuilder stringBuilder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(name))
                stringBuilder.AppendLine("Заполните имя");
            if (string.IsNullOrWhiteSpace(login))
                stringBuilder.AppendLine("Заполните логин");
            if (string.IsNullOrWhiteSpace(password))
                stringBuilder.AppendLine("Пароль не должен быть пустым");
            if (string.IsNullOrWhiteSpace(repeatPassword))
                stringBuilder.AppendLine("Повторите пароль");
            if (string.IsNullOrWhiteSpace(email))
                stringBuilder.AppendLine("Заполните email");
            if (string.IsNullOrWhiteSpace(phone))
                stringBuilder.AppendLine("Заполните телефон");
            if (password != repeatPassword)
                stringBuilder.AppendLine("Пароли не совпадают");
            if (birthDate.HasValue && birthDate.Value > DateTime.Today)
                stringBuilder.AppendLine("Дата рождения не может быть в будущем");

            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

  
            if (AppConnect.BookstoreModel.Users.Any(u => u.Login == login))
            {
                MessageBox.Show("Такой логин уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

  
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Некорректный email", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPhone(phone))
            {
                MessageBox.Show("Некорректный телефон. Допустимы только цифры и знак +", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Пароль должен содержать минимум 8 символов, включая цифру и букву", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            _currentUser.FullName = name;
            _currentUser.Login = login;
            _currentUser.Password = password;
            _currentUser.Email = email;
            _currentUser.BirthDate = birthDate;
            _currentUser.Phone = phone;
            _currentUser.RoleID = 2;

            try
            {
                AppConnect.BookstoreModel.Users.Add(_currentUser);
                AppConnect.BookstoreModel.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                AppData.MainFrame.FrameMain.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            return Regex.IsMatch(email, pattern);
        }

        private bool IsValidPhone(string phone)
        {
            string pattern = @"^\+?\d+$";
            return Regex.IsMatch(phone, pattern);
        }

        private bool IsValidPassword(string password)
        {
            string pattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$";
            return Regex.IsMatch(password, pattern);
        }
        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void emailTb_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.GoBack();
        }
    }
}

  
