using PdfSharp.Drawing;
using PdfSharp.Pdf;
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
using static WpfApp1.AppData.Books;
using System.IO;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для Payment.xaml
    /// </summary>
    public partial class Payment : Page
    {
        private OrderSummary _summary;

        public Payment(OrderSummary summary)
        {
            InitializeComponent();
            _summary = summary;

            OrderIdTextBlock.Text = $"Оплата заказа";
            SummaryBlock.Text = $"{_summary.TotalItems} товар(ов)";
            DiscountText.Text = $"Скидка: -{_summary.DiscountTotal:0.00} ₽";
            TotalText.Text = $"Итого: {_summary.TotalPrice:0.00} ₽";
            BonusText.Text = $"Бонусов за покупку: +{_summary.BonusEarned}";
            ConfirmButton.IsEnabled = true;

            var user = App.CurrentUser;
            if (user != null)
            {
                RecipientNameBox.Text = user.FullName;
                RecipientEmailBox.Text = user.Email;
                RecipientPhoneBox.Text = user.Phone;

                var favoriteCount = AppConnect.BookstoreModel.FavoriteBooks.Count(f => f.UserID == user.UserID);
                FavoriteCounter.Text = favoriteCount.ToString();
            }

            CartCounter.Text = App.CartBooks.Count.ToString();
        }

        private void GoToFavorites_Click(object sender, MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageFavoriteBooks());
        }

        private void GoToCart_Click(object sender, MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new Basket());
        }

        private void GoToCatalog_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageProducts());
        }
        private void GoToPersonalData_Click(object sender, MouseButtonEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PersonalAccount());
        }

        private void GoToOrders_Click(object sender, MouseButtonEventArgs e)
        {
            var page = new PersonalAccount();
            AppData.MainFrame.FrameMain.Navigate(page);

        
            page.PanelPersonalData.Visibility = Visibility.Collapsed;
            page.PanelOrders.Visibility = Visibility.Visible;
            page.PanelDiscounts.Visibility = Visibility.Collapsed;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var context = AppConnect.BookstoreModel;
            var user = App.CurrentUser;

            if (user == null)
            {
                MessageBox.Show("Пользователь не авторизован.");
                return;
            }

            try
            {
        
                var order = new Orders
                {
                    UserID = user.UserID,
                    OrderDate = DateTime.Now
                };
                context.Orders.Add(order);
                context.SaveChanges();

             
                var groupedBooks = App.CartBooks
                    .GroupBy(b => b.BookID)
                    .Select(g => new { BookID = g.Key, Quantity = g.Count() });

                foreach (var item in groupedBooks)
                {
                    context.OrderDetails.Add(new OrderDetails
                    {
                        OrderID = order.OrderID,
                        BookID = item.BookID,
                        Quantity = item.Quantity
                    });

                    var book = context.Books.FirstOrDefault(b => b.BookID == item.BookID);
                    if (book != null)
                        book.Stock -= item.Quantity;
                }

                context.SaveChanges();

             
                var booksForReceipt = App.CartBooks
                    .GroupBy(b => b.BookID)
                    .Select(g => new { Title = g.First().Title, Quantity = g.Count() })
                    .ToList();

          
                string city = CityBox.Text;

                string deliveryMethod = PickupOption.IsChecked == true
                    ? PickupOption.Content.ToString()
                    : StoreOption.Content.ToString();

                string paymentMethod = PayOnDeliveryOption.IsChecked == true
                    ? PayOnDeliveryOption.Content.ToString()
                    : PayOnlineOption.Content.ToString();

                string productList = string.Join("\n", booksForReceipt
                    .Select(b => $"• {b.Title} × {b.Quantity}"));
                GenerateReceiptPdf(
                    order.OrderID,
                    _summary.TotalPrice,
                    _summary.BonusEarned,
                    App.CartBooks,
                    city,
                    deliveryMethod,
                    paymentMethod
                );

             
                App.CartBooks.Clear();

                string receiptText = $"ЧЕК ОПЛАТЫ\n" +
                                     $"Заказ №{order.OrderID}\n" +
                                     $"Имя: {user.FullName}\n" +
                                     $"Дата: {DateTime.Now:dd.MM.yyyy}\n" +
                                     $"Время: {DateTime.Now:HH:mm:ss}\n" +
                                     $"Сумма: {_summary.TotalPrice:0.00} ₽\n" +
                                     $"Товары:\n{productList}\n" +
                                     $"Город: {city}\n" +
                                     $"Получение: {deliveryMethod}\n" +
                                     $"Оплата: {paymentMethod}";

                AppData.MainFrame.FrameMain.Navigate(new PageQRCode(receiptText));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при оформлении заказа: " + ex.Message);
            }
        }



        private void GenerateReceiptPdf(int orderId, decimal totalPrice, int bonus, List<Books> books, string city, string delivery, string payment)

        {
            var user = App.CurrentUser;
            if (user == null) return;

            PdfDocument document = new PdfDocument();
            document.Info.Title = "Чек оплаты";
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont titleFont = new XFont("Verdana", 16, XFontStyle.Bold);
            XFont regularFont = new XFont("Verdana", 12, XFontStyle.Regular);
            double y = 40;

            gfx.DrawString("КАССОВЫЙ ЧЕК", titleFont, XBrushes.Black, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);
            y += 40;

            gfx.DrawString($"Заказ №{orderId}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Имя: {user.FullName}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Дата: {DateTime.Now:dd.MM.yyyy}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Время: {DateTime.Now:HH:mm:ss}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Город: {city}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Получение: {delivery}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Оплата: {payment}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;

            gfx.DrawString("------------------------------------------------------------", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString("Товары:", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;

            foreach (var group in books.GroupBy(b => b.BookID))
            {
                var book = group.First();
                int quantity = group.Count();
                string line = $"• {book.Title} ({book.Authors?.FullName ?? "Автор не указан"}) — {book.Price:0.00} ₽ × {quantity}";
                gfx.DrawString(line, regularFont, XBrushes.Black, new XRect(50, y, page.Width, 20), XStringFormats.TopLeft);
                y += 20;
            }

            y += 10;
            gfx.DrawString("------------------------------------------------------------", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Итого: {totalPrice:0.00} ₽", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 25;
            gfx.DrawString($"Начислено бонусов: +{bonus}", regularFont, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft); y += 35;
            gfx.DrawString("СПАСИБО ЗА ПОКУПКУ!", titleFont, XBrushes.Black, new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);

            string filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Чек_оплаты.pdf");

            document.Save(filePath);
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(filePath) { UseShellExecute = true });
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService?.CanGoBack == true)
                NavigationService.GoBack();
        }

    }
}