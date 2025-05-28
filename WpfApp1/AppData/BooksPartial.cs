using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace WpfApp1.AppData
{
    public partial class Books
    {
        public string ActiveDiscountText
        {
            get
            {
                var today = DateTime.Today;
                var discount = this.Discounts
                    .FirstOrDefault(d => d.StartDate <= today && d.EndDate >= today);

                return discount != null ? $"Скидка {discount.DiscountPercent}%" : null;
            }
        }
        public class OrderSummary
        {
            public int OrderId { get; set; }
            public int TotalItems { get; set; }
            public decimal DiscountTotal { get; set; }
            public decimal TotalPrice { get; set; }
            public int BonusEarned { get; set; }
            public string DeliveryStatus { get; set; }
        }

        public bool IsInCart { get; set; } 
        public bool IsFavorite { get; set; }

        public bool IsSeriesBoxSet =>
            SeriesID != null && Title != null &&
            (Title.ToLower().Contains("подарочное издание") ||
             Title.ToLower().Contains("сборник") ||
             Title.ToLower().Contains("в одном томе")) &&
            (SeriesOrder == null || SeriesOrder == 1);

        public bool IsIndividualGift =>
            Title != null &&
            (Title.ToLower().Contains("подарочная версия") ||
             Title.ToLower().Contains("в подарочном оформлении") ||
             Title.ToLower().Contains("подарочный вариант"));

        public string SeriesDisplay =>
            BookSeries != null ? $"{BookSeries.SeriesName} (книга {SeriesOrder})" : "Одиночная книга";

        public BitmapImage CoverImage
        {
            get
            {
                string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                string defaultImagePath = Path.Combine(imageFolder, "noimage.png");

                if (BookImage != null && !string.IsNullOrEmpty(BookImage.ImagePath))
                {
                    string fullPath = Path.Combine(imageFolder, BookImage.ImagePath);
                    if (File.Exists(fullPath))
                    {
                        try
                        {
                            return new BitmapImage(new Uri(fullPath, UriKind.Absolute));
                        }
                        catch { }
                    }
                }

                return File.Exists(defaultImagePath)
                    ? new BitmapImage(new Uri(defaultImagePath, UriKind.Absolute))
                    : null;
            }
        }
        public bool HasDiscount
        {
            get
            {
                var today = DateTime.Today;
                return Discounts != null && Discounts.Any(d => d.StartDate <= today && d.EndDate >= today);
            }
        }

        public decimal DiscountedPrice
        {
            get
            {
                var today = DateTime.Today;
                var discount = Discounts?.FirstOrDefault(d => d.StartDate <= today && d.EndDate >= today);

                if (discount != null && Price.HasValue)
                {
                    decimal discounted = Price.Value * (1 - (decimal)discount.DiscountPercent / 100);
                    return Math.Round(discounted, 2);
                }

                return Price ?? 0;
            }
        }
        public string AgeRatingLabel => AgeRatings?.Label ?? "";

        public class ReceiptInfo
        {
            public int OrderId { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalPrice { get; set; }
        }
    }
}
