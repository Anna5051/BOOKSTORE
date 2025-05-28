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
    /// Логика взаимодействия для Description.xaml
    /// </summary>
    public partial class Description : Page
    {
        private Books _book;

    

        private void LoadBookInfo()
        {
            AgeRatingBlock.Text = _book.AgeRatings?.Label ?? "-";
            var context = AppConnect.BookstoreModel;
            var user = App.CurrentUser;
            if (user != null)
            {
                _book.IsFavorite = context.FavoriteBooks
                    .Any(f => f.BookID == _book.BookID && f.UserID == user.UserID);
            }

            TitleBlock.Text = _book.Title;
            AuthorBlock.Text = $"Автор: {_book.Authors?.FullName ?? "Неизвестен"}";
            YearBlock.Text = _book.YearPublished?.ToString() ?? "-";
            PriceBlock.Text = $"{_book.Price:0.00} ₽";
            StockBlock.Text = _book.Stock.ToString();
            SeriesBlock.Text = _book.SeriesDisplay;
            DescriptionBlock.Text = string.IsNullOrEmpty(_book.Description) ? "Описание отсутствует." : _book.Description;

            if (_book.CoverImage != null)
                CoverImage.Source = _book.CoverImage;

 
            DataContext = null;
            DataContext = _book;
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (_book.Stock > App.CartBooks.Count(b => b.BookID == _book.BookID))
            {
                App.CartBooks.Add(_book);
                MessageBox.Show("Книга добавлена в корзину.");
            }
            else
            {
                MessageBox.Show("Нельзя добавить больше книг, чем есть на складе.", "Ограничение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CartButton_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService?.Navigate(new Basket());
        }

        private void Heart_Click(object sender, MouseButtonEventArgs e)
        {
            var context = AppConnect.BookstoreModel;
            var user = App.CurrentUser;
            if (user == null) return;

            var existing = context.FavoriteBooks.FirstOrDefault(f => f.BookID == _book.BookID && f.UserID == user.UserID);
            if (existing != null)
            {
                context.FavoriteBooks.Remove(existing);
                _book.IsFavorite = false;
            }
            else
            {
                context.FavoriteBooks.Add(new FavoriteBooks
                {
                    BookID = _book.BookID,
                    UserID = user.UserID,
                    DateAdded = DateTime.Now
                });
                _book.IsFavorite = true;
            }

            context.SaveChanges();

      
            DataContext = null;
            DataContext = _book;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
   

            AppData.MainFrame.FrameMain.Navigate(new PageProducts(refreshNeeded: true));
        }
        private void SubmitReview_Click(object sender, RoutedEventArgs e)
        {
            var user = App.CurrentUser;
            var context = AppConnect.BookstoreModel;

            if (user == null)
            {
                MessageBox.Show("Вы должны войти в систему, чтобы оставить отзыв.");
                return;
            }

            if (RatingComboBox.SelectedItem is ComboBoxItem selectedRating &&
                int.TryParse(selectedRating.Content.ToString(), out int rating) &&
                rating >= 1 && rating <= 5)
            {
                var reviewText = CommentBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(reviewText))
                {
                    MessageBox.Show("Пожалуйста, введите текст отзыва.");
                    return;
                }

                var newReview = new Reviews
                {
                    BookID = _book.BookID,
                    UserID = user.UserID,
                    Rating = rating,
                    Comment = reviewText,
                    ReviewDate = DateTime.Now
                };

                context.Reviews.Add(newReview);
                context.SaveChanges();

                MessageBox.Show("Отзыв добавлен!");

        
                CommentBox.Clear();
                RatingComboBox.SelectedIndex = -1;

         
                LoadReviews();
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите оценку от 1 до 5.");
            }
        }

        private void LoadReviews()
        {
            var context = AppConnect.BookstoreModel;
            var currentUser = App.CurrentUser;
            bool isAdmin = currentUser != null && currentUser.RoleID == 1;

            var rawReviews = context.Reviews
                .Where(r => r.BookID == _book.BookID)
                .OrderByDescending(r => r.ReviewDate)
                .ToList();

            var reviews = rawReviews.Select(r => new
            {
                ReviewID = r.ReviewID, 
                User = context.Users.FirstOrDefault(u => u.UserID == r.UserID)?.FullName ?? "Аноним",
                Date = r.ReviewDate?.ToString("dd.MM.yyyy") ?? "-",
                Stars = new string('★', r.Rating ?? 0),
                r.Comment,
                IsAdminVisible = isAdmin ? Visibility.Visible : Visibility.Collapsed 
            }).ToList();

            ReviewList.ItemsSource = reviews;
        }

        public Description(Books book)
        {
            InitializeComponent();
            _book = book;
            LoadBookInfo();
            LoadReviews(); 
            UpdateRatingStats();
        }
        private void DeleteReview_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && int.TryParse(btn.Tag?.ToString(), out int reviewId))
            {
                var context = AppConnect.BookstoreModel;
                var review = context.Reviews.FirstOrDefault(r => r.ReviewID == reviewId);
                if (review != null)
                {
                    var result = MessageBox.Show("Удалить этот отзыв?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        context.Reviews.Remove(review);
                        context.SaveChanges();

                        LoadReviews();          
                        UpdateRatingStats();   

                        MessageBox.Show("Отзыв удалён.");
                    }
                }
            }
        }
        private void UpdateRatingStats()
        {
            var context = AppConnect.BookstoreModel;
            var reviews = context.Reviews.Where(r => r.BookID == _book.BookID).ToList();

            if (reviews.Count == 0)
            {
                AverageRatingBlock.Text = "–";
                RatingCountBlock.Text = "Нет оценок";
                ReviewSummaryTitle.Text = "ОТЗЫВЫ";
                StarIconsPanel.ItemsSource = Enumerable.Repeat(Brushes.LightGray, 5).ToList();
                StarsPanel.Children.Clear();
                return;
            }

            double avg = reviews.Average(r => r.Rating ?? 0);
            int count = reviews.Count;

            AverageRatingBlock.Text = avg.ToString("0.0");
            RatingCountBlock.Text = $"{count} оценок";
            ReviewSummaryTitle.Text = $"ОТЗЫВЫ  {count}";


            var stars = new List<Brush>();
            for (int i = 1; i <= 5; i++)
            {
                if (avg >= i)
                    stars.Add(Brushes.Gold);
                else if (avg >= i - 0.5)
                    stars.Add(new SolidColorBrush(Color.FromRgb(255, 215, 0)));
                else
                    stars.Add(Brushes.LightGray);
            }
            StarIconsPanel.ItemsSource = stars;

            var ratingGroups = reviews.GroupBy(r => r.Rating ?? 0).ToDictionary(g => g.Key, g => g.Count());

            StarsPanel.Children.Clear();

            for (int i = 5; i >= 1; i--)
            {
                int starCount = ratingGroups.ContainsKey(i) ? ratingGroups[i] : 0;

                var row = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 2, 0, 2) };
                row.Children.Add(new TextBlock
                {
                    Text = new string('★', i),
                    FontSize = 16,
                    Foreground = Brushes.Black,
                    Width = 100, 
                    TextAlignment = TextAlignment.Right,
                    Margin = new Thickness(0, 0, 10, 0) 
                });


                var bar = new ProgressBar
                {
                    Width = 200,
                    Height = 10,
                    Maximum = count,
                    Value = starCount,
                    Foreground = Brushes.DeepSkyBlue,
                    Background = Brushes.LightGray,
                    Margin = new Thickness(5, 0, 5, 0)
                };
                row.Children.Add(bar);

                row.Children.Add(new TextBlock
                {
                    Text = starCount.ToString(),
                    FontSize = 14,
                    Width = 30
                });

                StarsPanel.Children.Add(row);
            }
        }

    }

}

