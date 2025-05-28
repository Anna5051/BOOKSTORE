using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.AppData;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool NotificationsViewed { get; set; } = false;
        public static bool HasShownBackInStockNotification { get; set; } = false;
        public static Dictionary<int, int> BookStockCache = new Dictionary<int, int>();

        public static bool IsFavoriteNotificationClosed { get; set; } = false;
        public static Users CurrentUser { get; set; }
        public static List<Books> CartBooks = new List<Books>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeComponent();

         
            NotificationsViewed = false;
            HasShownBackInStockNotification = false;
            IsFavoriteNotificationClosed = false;

            CartBooks = new List<Books>();
            BookStockCache = new Dictionary<int, int>();
        }

        /// <summary>
        /// Проверка появления новых уведомлений (скидки и возврат книг в наличии).
        /// </summary>
        public static void CheckForNewNotifications()
        {
            var context = AppConnect.BookstoreModel;
            var today = DateTime.Today;

            bool hasDiscounts = context.Discounts.Any(d => d.EndDate >= today);
            if (hasDiscounts)
                NotificationsViewed = false;

            var user = CurrentUser;
            if (user != null && !IsFavoriteNotificationClosed)
            {
                bool backInStock = context.FavoriteBooks
                    .Any(f => f.UserID == user.UserID && f.Books.Stock > 0);

                if (backInStock)
                    NotificationsViewed = false;
            }
        }

    }
}