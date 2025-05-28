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
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Page
    {
        public Basket()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            StackPanelCart.Children.Clear();
            decimal total = 0;
            int bonusEarned = 0;
            int totalCount = 0;

            if (!App.CartBooks.Any())
            {
                StackPanelCartSection.Visibility = Visibility.Collapsed;
                SummaryPanel.Visibility = Visibility.Collapsed;
                StackPanelEmptyCart.Visibility = Visibility.Visible;
                TextItems.Text = "";
                return;
            }

            StackPanelCartSection.Visibility = Visibility.Visible;
            SummaryPanel.Visibility = Visibility.Visible;
            StackPanelEmptyCart.Visibility = Visibility.Collapsed;

            var bookGroups = App.CartBooks
                .GroupBy(b => b.BookID)
                .Select(g =>
                {
                    int firstIndex = App.CartBooks.FindIndex(b => b.BookID == g.Key);
                    return new
                    {
                        Book = g.First(),
                        Quantity = g.Count(),
                        Index = firstIndex
                    };
                })
                .OrderBy(x => x.Index)
                .ToList();

            foreach (var item in bookGroups)
            {
                var book = item.Book;
                int quantity = item.Quantity;
                totalCount += quantity;

                decimal price = book.Price ?? 0;
                decimal discountedPrice = book.DiscountedPrice;
                decimal discount = book.HasDiscount ? (price - discountedPrice) / price * 100 : 0;


                var container = new Border
                {
                    Background = Brushes.White,
                    BorderBrush = (Brush)new BrushConverter().ConvertFrom("#DCE3EC"),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(10),
                    Margin = new Thickness(0, 0, 0, 15),
                    Padding = new Thickness(12),
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };


                var innerGrid = new Grid();
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120) });
                innerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var image = new Image
                {
                    Source = book.CoverImage,
                    Width = 100,
                    Height = 150,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Top
                };
                Grid.SetColumn(image, 0);

                var infoPanel = new StackPanel { Margin = new Thickness(10, 0, 0, 0) };
                infoPanel.Children.Add(new TextBlock { Text = book.Title, FontWeight = FontWeights.Bold, FontSize = 18, TextWrapping = TextWrapping.Wrap });
                infoPanel.Children.Add(new TextBlock { Text = book.Authors?.FullName ?? "", Foreground = Brushes.Gray, FontSize = 14 });

                if (discount > 0)
                {
                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = $"{discountedPrice * quantity:0.00} ₽",
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Red,
                        FontSize = 16
                    });
                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = $"{price * quantity:0.00} ₽",
                        TextDecorations = TextDecorations.Strikethrough,
                        Foreground = Brushes.Gray,
                        FontSize = 13
                    });
                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = $"Скидка {discount}%",
                        Foreground = Brushes.Green,
                        FontSize = 13
                    });
                }
                else
                {
                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = $"{price * quantity:0.00} ₽",
                        FontWeight = FontWeights.Bold,
                        Foreground = Brushes.Red,
                        FontSize = 16
                    });
                }

                if (book.IsSeriesBoxSet)
                {
                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = "🎁 Подарочное издание всей серии",
                        Foreground = Brushes.DarkRed,
                        FontWeight = FontWeights.Bold,
                        FontSize = 13,
                        Margin = new Thickness(0, 6, 0, 0)
                    });
                }
                else if (book.IsIndividualGift)
                {
                    infoPanel.Children.Add(new TextBlock
                    {
                        Text = "🎀 Подарочная книга",
                        Foreground = Brushes.DarkOrange,
                        FontSize = 13,
                        Margin = new Thickness(0, 6, 0, 0)
                    });
                }

                var controlPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 12, 0, 0) };
                var txtQty = new TextBlock
                {
                    Text = quantity.ToString(),
                    Width = 30,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 14
                };

                var btnMinus = new Button { Content = "-", Width = 30 };
                btnMinus.IsEnabled = quantity > 1;
                btnMinus.Foreground = quantity > 1 ? Brushes.Black : Brushes.Gray;
                btnMinus.Click += (s, e) =>
                {
                    int removeIndex = App.CartBooks.FindIndex(b => b.BookID == book.BookID);
                    if (removeIndex >= 0)
                    {
                        App.CartBooks.RemoveAt(removeIndex);
                        LoadCart();
                        if (AppData.MainFrame.FrameMain.Content is PageProducts p) p.ResetCartUI();
                    }
                };

                var btnPlus = new Button { Content = "+", Width = 30 };
                btnPlus.Click += (s, e) =>
                {
                    if (App.CartBooks.Count(b => b.BookID == book.BookID) < book.Stock)
                    {
                        App.CartBooks.Add(book);
                    }
                    LoadCart();
                    if (AppData.MainFrame.FrameMain.Content is PageProducts p) p.ResetCartUI();
                };

                var heartPath = new Path
                {
                    Width = 18,
                    Height = 18,
                    Margin = new Thickness(10, 0, 0, 0),
                    Cursor = Cursors.Hand,
                    Stretch = Stretch.Uniform,
                    Tag = book,
                    Data = Geometry.Parse("M12 21.35l-1.45-1.32C5.4 15.36 2 12.28 2 8.5 " +
                                          "2 5.42 4.42 3 7.5 3c1.74 0 3.41 0.81 4.5 2.09" +
                                          "C13.09 3.81 14.76 3 16.5 3 " +
                                          "19.58 3 22 5.42 22 8.5c0 3.78-3.4 " +
                                          "6.86-8.55 11.54L12 21.35z"),
                    StrokeThickness = 1.5,
                    Fill = book.IsFavorite ? Brushes.Red : Brushes.Transparent,
                    Stroke = book.IsFavorite ? Brushes.Red : Brushes.Gray
                };

                heartPath.MouseDown += (s, e) =>
                {
                    var context = AppConnect.BookstoreModel;
                    var user = App.CurrentUser;
                    if (user == null) return;

                    var existing = context.FavoriteBooks.FirstOrDefault(f => f.BookID == book.BookID && f.UserID == user.UserID);
                    if (existing != null)
                    {
                        context.FavoriteBooks.Remove(existing);
                        book.IsFavorite = false;
                    }
                    else
                    {
                        context.FavoriteBooks.Add(new FavoriteBooks
                        {
                            BookID = book.BookID,
                            UserID = user.UserID,
                            DateAdded = DateTime.Now
                        });
                        book.IsFavorite = true;
                    }

                    context.SaveChanges();
                    LoadCart();
                    if (AppData.MainFrame.FrameMain.Content is PageProducts p) p.ResetCartUI();
                };

                var btnDelete = new Button { Content = "🗑", Width = 30, Margin = new Thickness(5, 0, 0, 0) };
                btnDelete.Click += (s, e) =>
                {
                    App.CartBooks.RemoveAll(b => b.BookID == book.BookID);
                    LoadCart();
                    if (AppData.MainFrame.FrameMain.Content is PageProducts p) p.ResetCartUI();
                };

                controlPanel.Children.Add(btnMinus);
                controlPanel.Children.Add(txtQty);
                controlPanel.Children.Add(btnPlus);
                controlPanel.Children.Add(heartPath);
                controlPanel.Children.Add(btnDelete);

                infoPanel.Children.Add(controlPanel);
                Grid.SetColumn(infoPanel, 1);

                innerGrid.Children.Add(image);
                innerGrid.Children.Add(infoPanel);
                container.Child = innerGrid;
                StackPanelCart.Children.Add(container);

                total += discountedPrice * quantity;
                bonusEarned += (int)Math.Floor(discountedPrice * quantity * 0.03m);
            }

            TextItems.Text = $"({totalCount} товар{(totalCount == 1 ? "" : totalCount < 5 ? "а" : "ов")})";
            TextTotalItems.Text = $"{totalCount} товар{(totalCount == 1 ? "" : totalCount < 5 ? "а" : "ов")}";
            TextTotal.Text = $"{total:0.00} ₽";


        }

        private decimal GetDiscountPercent(int bookId)
        {
            var now = DateTime.Now;

            var discount = AppConnect.BookstoreModel.Discounts
                .AsNoTracking() 
                .FirstOrDefault(d => d.BookID == bookId && d.StartDate <= now && d.EndDate >= now);

            return discount?.DiscountPercent ?? 0;
        }



        private void GoToCatalog_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageProducts());
        }

        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {
            if (App.CartBooks.Any())
            {
                if (MessageBox.Show("Очистить корзину?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    App.CartBooks.Clear();
                    LoadCart();
                    if (AppData.MainFrame.FrameMain.Content is PageProducts p) p.ResetCartUI();
                }
            }
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var user = App.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            if (!App.CartBooks.Any())
            {
                MessageBox.Show("Корзина пуста.");
                return;
            }

            decimal total = 0;
            int bonusEarned = 0;
            int totalCount = 0;

            var groupedBooks = App.CartBooks
                .GroupBy(b => b.BookID)
                .Select(g => new { Book = g.First(), Quantity = g.Count() });

            foreach (var item in groupedBooks)
            {
                var book = item.Book;
                int quantity = item.Quantity;

                decimal price = book.Price ?? 0;
                decimal discount = GetDiscountPercent(book.BookID);
                decimal discountedPrice = price * (1 - discount / 100);

                total += discountedPrice * quantity;
                bonusEarned += (int)Math.Floor(discountedPrice * quantity * 0.03m);
                totalCount += quantity;
            }

            var summary = new WpfApp1.AppData.Books.OrderSummary
            {
                OrderId = 0,
                TotalItems = totalCount,
                DiscountTotal = total > 1000 ? 100 : 0,
                TotalPrice = total,
                BonusEarned = bonusEarned,
                DeliveryStatus = "Не выбрана"
            };

            AppData.MainFrame.FrameMain.Navigate(new Payment(summary));
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.GoBack();
        }

    }
}