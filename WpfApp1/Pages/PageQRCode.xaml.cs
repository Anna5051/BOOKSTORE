using System;
using System.Collections.Generic;
using System.IO;
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
using ZXing;
using ZXing.Common;

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для PageQRCode.xaml
    /// </summary>
    public partial class PageQRCode : Page
    {
        public PageQRCode(string receiptText)
        {
            InitializeComponent();
            GenerateQRCode(receiptText);
        }

        private void GenerateQRCode(string text)
        {
            var options = new EncodingOptions
            {
                Width = 300,
                Height = 300,
                Margin = 1
            };

            options.Hints[EncodeHintType.CHARACTER_SET] = "UTF-8";

            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = options
            };

            var result = writer.Write(text);
            using (var memoryStream = new MemoryStream())
            {
                result.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                memoryStream.Position = 0;

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = memoryStream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();

                imgQr.Source = bitmap;
            }
        }

        private void GoToCatalog_Click(object sender, RoutedEventArgs e)
        {
            AppData.MainFrame.FrameMain.Navigate(new PageProducts());
        }
    }
}

//Install-Package PdfSharp     Install-Package PdfSharp//