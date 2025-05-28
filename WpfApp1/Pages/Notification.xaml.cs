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
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для Notification.xaml
    /// </summary>
    public partial class Notification : Page
    {
        public Notification()
        {
            InitializeComponent();
            App.NotificationsViewed = true;
            LoadDiscountNotifications();
        }

        private void LoadDiscountNotifications()
        {
            var context = AppConnect.BookstoreModel;
            var today = DateTime.Today;

     
            var discounts = context.Discounts
                .Where(d => d.EndDate >= today && d.Books != null)
                .AsEnumerable()
                .Select(d => $"Промокод на {d.DiscountPercent}% для книги «{d.Books.Title}» — действует до {d.EndDate:dd.MM.yyyy}")
                .ToList();

            DiscountList.ItemsSource = discounts;

            var user = App.CurrentUser;
            if (user != null && !App.IsFavoriteNotificationClosed)
            {
                bool anyNowInStock = context.FavoriteBooks
                    .Where(f => f.UserID == user.UserID)
                    .Any(f => f.Books.Stock > 0 && f.Books.Stock < 1000);

                FavoriteNotificationBanner.Visibility = anyNowInStock ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                FavoriteNotificationBanner.Visibility = Visibility.Collapsed;
            }
        }

        private void CloseFavoriteNotification_Click(object sender, RoutedEventArgs e)
        {
            App.IsFavoriteNotificationClosed = true;
            FavoriteNotificationBanner.Visibility = Visibility.Collapsed;
        }

        private void CloseDiscountItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is string discountText)
            {
                var list = DiscountList.ItemsSource as List<string>;
                if (list != null)
                {
                    list.Remove(discountText);
                    DiscountList.ItemsSource = null;
                    DiscountList.ItemsSource = list;
                    if (list.Count == 0)
                    {
                        App.NotificationsViewed = true;

                        if (AppData.MainFrame.FrameMain.Content is PageProducts mainPage)
                            mainPage.UpdateNotificationCounter();
                    }
                }
            }
        }

        public static int GetTotalNotificationsCount()
        {
            var context = AppConnect.BookstoreModel;
            var today = DateTime.Today;
            int count = 0;

           
            if (!App.NotificationsViewed)
            {
                count += context.Discounts.Count(d => d.EndDate >= today);

                if (!App.IsFavoriteNotificationClosed)
                {
                    var user = App.CurrentUser;
                    bool anyNowInStock = user != null &&
                        context.FavoriteBooks.Any(f => f.UserID == user.UserID && f.Books.Stock > 0);

                    if (anyNowInStock)
                        count += 1;
                }
            }

            return count;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!IsVisible) 
            {
                App.NotificationsViewed = true;

            
                if (AppData.MainFrame.FrameMain.Content is PageProducts mainPage)
                    mainPage.UpdateNotificationCounter();
            }
        }

    }
}
