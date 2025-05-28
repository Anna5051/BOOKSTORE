using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для PersonalAccount.xaml
    /// </summary>
    public partial class PersonalAccount : Page
    {
        private string currentPassword = "";
        private List<Orders> rawOrders = new List<Orders>();

        public PersonalAccount()
        {
            InitializeComponent();
            LoadUserInfo();

            if (App.CurrentUser?.RoleID == 2)
            {
                DiscountsSection.Visibility = Visibility.Collapsed;
            }
        }
        private void LoadUserInfo()
        {
            var user = App.CurrentUser;
            if (user != null)
            {
                var nameParts = user.FullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length > 0) tbSurname.Text = nameParts[0];
                if (nameParts.Length > 1) tbName.Text = nameParts[1];

                tbLogin.Text = user.Login;
                tbEmail.Text = user.Email;
                tbPhone.Text = user.Phone;
                dpBirthDate.SelectedDate = user.BirthDate;
                pbPassword.Password = user.Password;
                pbConfirmPassword.Password = user.Password;

                currentPassword = user.Password;
            }
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                UpdateCounters();
                LoadUserInfo();
            }
        }

        private void GoToFavorites_Click(object sender, MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageFavoriteBooks());
        }

        private void GoToCart_Click(object sender, MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new Basket());
        }

        private void UpdateCounters()
        {
            var userId = App.CurrentUser?.UserID ?? 0;
            FavoriteCounter.Text = AppConnect.BookstoreModel.FavoriteBooks.Count(f => f.UserID == userId).ToString();
            CartCounter.Text = App.CartBooks.Count.ToString();
        }

        private void cbShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Ваш пароль: {pbPassword.Password}", "Показать пароль", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void cbShowPassword_Unchecked(object sender, RoutedEventArgs e) { }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbSurname.Text) || string.IsNullOrWhiteSpace(tbName.Text) || dpBirthDate.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (pbPassword.Password != pbConfirmPassword.Password)
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = App.CurrentUser;
            if (user != null)
            {
                user.FullName = $"{tbSurname.Text} {tbName.Text}";
                user.Email = tbEmail.Text;
                user.BirthDate = dpBirthDate.SelectedDate;
                user.Password = pbPassword.Password;

                try
                {
                    AppConnect.BookstoreModel.SaveChanges();
                    MessageBox.Show("Изменения успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void GoToCatalog_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageProducts());
        }

        private void GoToPersonalData_Click(object sender, MouseButtonEventArgs e)
        {
            HideAllPanels();
            PanelPersonalData.Visibility = Visibility.Visible;
        }
        private void GoToOrders_Click(object sender, MouseButtonEventArgs e)
        {
            HideAllPanels(); 
            PanelOrders.Visibility = Visibility.Visible;

            var userId = App.CurrentUser?.UserID ?? 0;

            rawOrders = AppConnect.BookstoreModel.Orders
                .Where(o => o.UserID == userId)
                .Include(o => o.OrderDetails.Select(od => od.Books.BookImage))
                .ToList();

            ApplyOrderSorting();
        }
        private void SortOrdersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PanelOrders.Visibility == Visibility.Visible)
                ApplyOrderSorting();
        }

        private void ApplyOrderSorting()
        {
            IEnumerable<Orders> sortedOrders;

            if (SortOrdersComboBox.SelectedIndex == 0) 
                sortedOrders = rawOrders.OrderByDescending(o => o.OrderDate);
            else
                sortedOrders = rawOrders.OrderBy(o => o.OrderDate);

            var orders = sortedOrders.Select(o => new
            {
                o.OrderID,
                o.OrderDate,
                TotalSum = o.OrderDetails.Sum(d => d.Quantity * d.Books.Price),
                Items = o.OrderDetails.Select(d => new
                {
                    d.Books.Title,
                    d.Quantity,
                    d.Books.Price,
                    d.Books.CoverImage
                }).ToList()
            }).ToList();

            OrdersList.ItemsSource = orders;
        }


        private void DeleteOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int orderId))
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить заказ №{orderId}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result != MessageBoxResult.Yes) return;

                try
                {
                    var context = AppConnect.BookstoreModel;

                    var payments = context.Payments.Where(p => p.OrderID == orderId).ToList();
                    context.Payments.RemoveRange(payments);

                    var details = context.OrderDetails.Where(d => d.OrderID == orderId).ToList();
                    context.OrderDetails.RemoveRange(details);

                    var order = context.Orders.FirstOrDefault(o => o.OrderID == orderId);
                    if (order != null)
                        context.Orders.Remove(order);

                    context.SaveChanges();

                    GoToOrders_Click(null, null);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении заказа: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void HideAllPanels()
        {
            PanelPersonalData.Visibility = Visibility.Collapsed;
            PanelOrders.Visibility = Visibility.Collapsed;
            PanelDiscounts.Visibility = Visibility.Collapsed;
        }

        private void GoToDiscounts_Click(object sender, MouseButtonEventArgs e)
        {
            HideAllPanels();
            PanelDiscounts.Visibility = Visibility.Visible;

            cbBooks.ItemsSource = AppConnect.BookstoreModel.Books.ToList();
            LoadDiscounts(); 
        }

        private void AddDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (cbBooks.SelectedItem is Books selectedBook &&
                int.TryParse(tbDiscountPercent.Text, out int percent) &&
                dpStart.SelectedDate is DateTime start &&
                dpEnd.SelectedDate is DateTime end)
            {
                if (start > end)
                {
                    MessageBox.Show("Дата начала не может быть позже даты окончания.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var discount = new Discounts
                {
                    BookID = selectedBook.BookID,
                    DiscountPercent = percent,
                    StartDate = start,
                    EndDate = end
                };
                AppConnect.BookstoreModel.Discounts.Add(discount);
                AppConnect.BookstoreModel.SaveChanges();

                MessageBox.Show("Скидка успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadDiscounts();

              
                if (AppData.MainFrame.FrameMain.Content is PageProducts mainPage)
                    mainPage.UpdateNotificationCounter();

            }
            else
            {
                MessageBox.Show("Заполните все поля корректно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void DeleteDiscount_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && int.TryParse(button.Tag.ToString(), out int discountId))
            {
                var result = MessageBox.Show("Удалить эту скидку?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result != MessageBoxResult.Yes)
                    return;

                try
                {
                    var context = AppConnect.BookstoreModel;

                    var discount = context.Discounts.FirstOrDefault(d => d.DiscountID == discountId);
                    if (discount != null)
                    {
                        context.Discounts.Remove(discount);
                        context.SaveChanges();
                        LoadDiscounts();
                        MessageBox.Show("Скидка удалена.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении скидки: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadDiscounts()
        {
            var context = AppConnect.BookstoreModel;
            var discounts = context.Discounts.Include(d => d.Books).ToList();
            DiscountsList.ItemsSource = discounts;
        }

    }
}
