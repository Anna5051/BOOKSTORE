using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp1.AppData;
using Word = Microsoft.Office.Interop.Word;

namespace WpfApp1.Pages
{
    public partial class PageFavoriteBooks : Page
    {
        public PageFavoriteBooks()
        {
            InitializeComponent();
            LoadFavoriteBooks();
        }

        private void LoadFavoriteBooks()
        {
            var currentUser = App.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            var favorites = AppConnect.BookstoreModel.FavoriteBooks
                .Where(fb => fb.UserID == currentUser.UserID)
                .Select(fb => new
                {
                    fb.FavoriteID,
                    Book = fb.Books,
                    fb.DateAdded
                })
                .ToList();

            bool anyNowInStock = false;

            foreach (var f in favorites)
            {
                int bookId = f.Book.BookID;
                int currentStock = f.Book.Stock ?? 0;

                if (App.BookStockCache.TryGetValue(bookId, out int prevStock))
                {
                    if (prevStock == 0 && currentStock > 0)
                        anyNowInStock = true;
                }

      
                App.BookStockCache[bookId] = currentStock;
            }


            NotificationBanner.Visibility = (anyNowInStock && !App.IsFavoriteNotificationClosed)
                ? Visibility.Visible
                : Visibility.Collapsed;

            ListFavoriteBooks.ItemsSource = favorites;
            CountRecords.Text = $"Найдено записей: {favorites.Count}";
            StackPanelEmptyFavorites.Visibility = favorites.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            ListFavoriteBooks.Visibility = favorites.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void UpdateFavoriteBooks()
        {
            var currentUser = App.CurrentUser;
            if (currentUser == null)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            var query = AppConnect.BookstoreModel.FavoriteBooks
                .Where(fb => fb.UserID == currentUser.UserID)
                .Select(fb => new
                {
                    fb.FavoriteID,
                    Book = fb.Books,
                    fb.DateAdded
                })
                .AsQueryable();

            string search = TextSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(f =>
                    f.Book.Title.ToLower().Contains(search) ||
                    f.Book.Authors.FullName.ToLower().Contains(search));
            }

            string sort = (ComboSort.SelectedItem as ComboBoxItem)?.Content.ToString();
            switch (sort)
            {
                case "Название А-Я":
                    query = query.OrderBy(f => f.Book.Title);
                    break;
                case "Название Я-А":
                    query = query.OrderByDescending(f => f.Book.Title);
                    break;
                case "Цена по возрастанию":
                    query = query.OrderBy(f => f.Book.Price);
                    break;
                case "Цена по убыванию":
                    query = query.OrderByDescending(f => f.Book.Price);
                    break;
            }

            var filtered = query.ToList();
            ListFavoriteBooks.ItemsSource = filtered;
            CountRecords.Text = $"Найдено записей: {filtered.Count}";
            StackPanelEmptyFavorites.Visibility = filtered.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            ListFavoriteBooks.Visibility = filtered.Count == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e) => UpdateFavoriteBooks();
        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateFavoriteBooks();

        private void RemoveFromFavorite_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.Tag is int favoriteId)
            {
                var favorite = AppConnect.BookstoreModel.FavoriteBooks.FirstOrDefault(f => f.FavoriteID == favoriteId);
                if (favorite != null)
                {
                    AppConnect.BookstoreModel.FavoriteBooks.Remove(favorite);
                    AppConnect.BookstoreModel.SaveChanges();
                    LoadFavoriteBooks();
                }
            }
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var book = button?.Tag as Books;
            if (book == null) return;

            if (App.CartBooks.Any(b => b.BookID == book.BookID))
            {
                AppData.MainFrame.FrameMain.Navigate(new Basket());
                return;
            }

            int inCart = App.CartBooks.Count(b => b.BookID == book.BookID);
            if (inCart < book.Stock)
            {
                App.CartBooks.Add(book);
                button.Content = "ОФОРМИТЬ";
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0b5ed7"));
                button.Foreground = Brushes.White;
            }
            else
            {
                MessageBox.Show("Нельзя добавить больше книг, чем есть на складе.", "Ограничение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btExportToWord_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Экспорт временно отключён для отладки.");
        }

        private void GoToCatalog_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageProducts());
        }

        private void CloseNotification_Click(object sender, RoutedEventArgs e)
        {
            App.IsFavoriteNotificationClosed = true;
            NotificationBanner.Visibility = Visibility.Collapsed;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

    }
}