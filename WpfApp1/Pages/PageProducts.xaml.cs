using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class PageProducts : Page
    {


        private bool _shouldRefresh = false;

        public PageProducts(bool refreshNeeded = false)
        {
            InitializeComponent();
            _shouldRefresh = refreshNeeded;

       
            if (App.CurrentUser?.RoleID == 2) 
            {
                bCreateRecipe.Visibility = Visibility.Collapsed;
                bEditRecipe.Visibility = Visibility.Collapsed;
                bDeleteRecipe.Visibility = Visibility.Collapsed;
            }
            App.CheckForNewNotifications();

            UpdateNotificationCounter();
            UpdateFavoriteCounter();
            UpdateCartCounter();

            var genres = AppConnect.BookstoreModel.Genres.Select(g => g.GenreName).ToList();
            genres.Insert(0, "Все жанры");
            ComboFilter.ItemsSource = genres;
            ComboFilter.SelectedIndex = 0;

            ListProducts.ItemsSource = AppConnect.BookstoreModel.Books.ToList();

            if (_shouldRefresh)
            {
                ResetCartUI();
            }
        }



        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateBooks();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateBooks();
        }

        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateBooks();
        }

        private void UpdateBooks(bool preserveOrder = false)
        {
            var books = AppConnect.BookstoreModel.Books.AsQueryable();

            var selectedGenre = ComboFilter.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedGenre) && selectedGenre != "Все жанры")
                books = books.Where(b => b.Genres.GenreName == selectedGenre);

            var searchText = TextSearch.Text.ToLower();
            if (!string.IsNullOrEmpty(searchText))
                books = books.Where(b =>
                    b.Title.ToLower().Contains(searchText) ||
                    b.Authors.FullName.ToLower().Contains(searchText));

            var result = books.ToList();

        
            if (!preserveOrder)
            {
                var selectedSort = (ComboSort.SelectedItem as ComboBoxItem)?.Content.ToString();
                switch (selectedSort)
                {
                    case "Название А-Я":
                        result = result.OrderBy(b => b.Title).ToList();
                        break;
                    case "Название Я-А":
                        result = result.OrderByDescending(b => b.Title).ToList();
                        break;
                    case "Цена ↑":
                        result = result.OrderBy(b => b.Price).ToList();
                        break;
                    case "Цена ↓":
                        result = result.OrderByDescending(b => b.Price).ToList();
                        break;
                    default:
                       
                        result = result.OrderBy(b => b.BookID).ToList();
                        break;
                }
            }
            
            foreach (var book in result)
            {
                book.IsFavorite = AppConnect.BookstoreModel.FavoriteBooks
                    .Any(f => f.BookID == book.BookID && f.UserID == App.CurrentUser.UserID);
           
            }

            ListProducts.ItemsSource = result;
            CountRecords.Text = $"Найдено записей: {result.Count}";
        }

        private void bEditRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (ListProducts.SelectedItem is Books selectedBook)
                NavigationService.Navigate(new CreateBook(selectedBook));
            else
                MessageBox.Show("Выберите книгу!");
        }

        private void bCreateRecipe_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateBook(null));
        }

        private void bDeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (ListProducts.SelectedItem is Books selectedBook)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить эту книгу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var context = AppConnect.BookstoreModel;

                        int? authorIdToRemove = selectedBook.AuthorID;

                
                        context.Reviews.RemoveRange(context.Reviews.Where(r => r.BookID == selectedBook.BookID));
                        context.FavoriteBooks.RemoveRange(context.FavoriteBooks.Where(f => f.BookID == selectedBook.BookID));
                        context.OrderDetails.RemoveRange(context.OrderDetails.Where(od => od.BookID == selectedBook.BookID));
                        context.Supplies.RemoveRange(context.Supplies.Where(s => s.BookID == selectedBook.BookID));
                        context.Discounts.RemoveRange(context.Discounts.Where(d => d.BookID == selectedBook.BookID));
                        context.Books.Remove(selectedBook);

                        context.SaveChanges();

               
                        if (authorIdToRemove.HasValue)
                        {
                            bool usedElsewhere = context.Books.Any(b => b.AuthorID == authorIdToRemove.Value);
                            if (!usedElsewhere)
                            {
                                var author = context.Authors.FirstOrDefault(a => a.AuthorID == authorIdToRemove.Value);
                                if (author != null)
                                {
                                    context.Authors.Remove(author);
                                    context.SaveChanges(); 
                                }
                            }
                        }

                        MessageBox.Show("Книга и её автор удалены (если он больше не используется).");
                        UpdateBooks();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении книги: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите книгу!");
            }
        }


        private void FavoriteRecipes_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageFavoriteBooks());
        }

        private void AddToFavorite(Books book)
        {
            var user = App.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            var favorite = new FavoriteBooks
            {
                UserID = user.UserID,
                BookID = book.BookID,
                DateAdded = DateTime.Now
            };

            AppConnect.BookstoreModel.FavoriteBooks.Add(favorite);
            AppConnect.BookstoreModel.SaveChanges();
            UpdateFavoriteCounter();
        }

        private void RemoveFromFavorite(Books book)
        {
            var user = App.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            var favorite = AppConnect.BookstoreModel.FavoriteBooks
                .FirstOrDefault(f => f.BookID == book.BookID && f.UserID == user.UserID);
            if (favorite != null)
            {
                AppConnect.BookstoreModel.FavoriteBooks.Remove(favorite);
                AppConnect.BookstoreModel.SaveChanges();
                UpdateFavoriteCounter();
            }
        }

        private void FavoriteIcon_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;

            var book = fe.Tag as Books;
            if (book == null) return;

            if (book.IsFavorite)
                RemoveFromFavorite(book);
            else
                AddToFavorite(book);

            UpdateBooks(preserveOrder: true);
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            var book = button.Tag as Books;
            if (book == null) return;

          
            if (App.CartBooks.Any(b => b.BookID == book.BookID))
            {
                NavigationService?.Navigate(new Basket());
                return;
            }

     
            int inCart = App.CartBooks.Count(b => b.BookID == book.BookID);
            if (inCart < book.Stock)
            {
                App.CartBooks.Add(book);
       
                UpdateCartCounter();

                button.Content = "ОФОРМИТЬ";
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0b5ed7"));
                button.Foreground = Brushes.White;
            }
            else
            {
                MessageBox.Show("Нельзя добавить больше книг, чем есть на складе.", "Ограничение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GoToBasket_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new Basket());
        }

        private void UpdateFavoriteCounter()
        {
            var count = AppConnect.BookstoreModel.FavoriteBooks
                .Count(f => f.UserID == App.CurrentUser.UserID);

            if (FavoriteCounter != null && FavoriteHeartIcon != null)
            {
                FavoriteCounter.Text = count.ToString();
                FavoriteHeartIcon.Fill = count > 0 ? Brushes.Red : Brushes.Gray;
            }
        }

        private void UpdateCartCounter()
        {
            if (CartCounter != null)
                CartCounter.Text = App.CartBooks.Count.ToString();
        }


        public void ResetCartUI()
        {
            UpdateCartCounter();
            UpdateBooks(); 
        }


        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsVisible)
            {
                ResetCartUI(); 
                UpdateFavoriteCounter(); 
                UpdateNotificationCounter(); 
            }
        }


        private void Title_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBlock tb && tb.DataContext is Books selectedBook)
            {
                AppData.MainFrame.FrameMain.Navigate(new Description(selectedBook));
            }
        }
        private void ProfileIcon_Click(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PersonalAccount());
        }
        private void NotificationIcon_Click(object sender, MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new Notification());
        }
        public void UpdateNotificationCounter()
        {
            int count = 0;

            var context = AppConnect.BookstoreModel;
            var user = App.CurrentUser;

            if (!App.NotificationsViewed)
            {
                if (user != null && !App.IsFavoriteNotificationClosed)
                {
                    bool anyNowInStock = context.FavoriteBooks
                        .Any(f => f.UserID == user.UserID && f.Books.Stock > 0);

                    if (anyNowInStock)
                        count += 1;
                }

               
                count += context.Discounts.Count(d => d.EndDate >= DateTime.Today);
            }

            NotificationCounter.Text = count.ToString();
            NotificationCounterBorder.Visibility = count > 0 ? Visibility.Visible : Visibility.Collapsed;
        }





        private void ListProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }

}