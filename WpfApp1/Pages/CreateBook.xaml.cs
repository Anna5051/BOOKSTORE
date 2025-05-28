using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp1.AppData;

namespace WpfApp1.Pages
{
    public partial class CreateBook : Page
    {
        public Books Book { get; set; }
        private string selectedImageFileName;

        public CreateBook(Books book = null)
        {
            InitializeComponent();

            var context = AppConnect.BookstoreModel;

            cbGenres.ItemsSource = context.Genres.ToList();
            cbGenres.DisplayMemberPath = "GenreName";
            cbGenres.SelectedValuePath = "GenreID";

            cbPublishers.ItemsSource = context.Publishers.ToList();
            cbPublishers.DisplayMemberPath = "Name";
            cbPublishers.SelectedValuePath = "PublisherID";

            cbAgeRatings.ItemsSource = context.AgeRatings.ToList();
            cbAgeRatings.DisplayMemberPath = "Label";
            cbAgeRatings.SelectedValuePath = "AgeRatingID";

            if (book != null)
            {
                Book = book;
                DataContext = Book;

                cbGenres.SelectedValue = Book.GenreID;
                cbPublishers.SelectedValue = Book.PublisherID;
                cbAgeRatings.SelectedValue = Book.AgeRatingID;

                tbSeriesName.Text = Book.BookSeries?.SeriesName ?? string.Empty;
                tbSeriesOrder.Text = Book.SeriesOrder?.ToString();

                var author = context.Authors.FirstOrDefault(a => a.AuthorID == Book.AuthorID);
                if (author != null)
                    tbAuthor.Text = author.FullName;

                if (Book.BookImage != null && !string.IsNullOrEmpty(Book.BookImage.ImagePath))
                {
                    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", Book.BookImage.ImagePath);
                    if (File.Exists(path))
                    {
                        imgCover.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
                        selectedImageFileName = Book.BookImage.ImagePath;
                    }
                }
            }
            else
            {
                Book = new Books();
                DataContext = Book;
            }
        }

        private void bImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Изображения (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Все файлы (*.*)|*.*"
            };

            if (dlg.ShowDialog() == true)
            {
                string selectedPath = dlg.FileName;
                selectedImageFileName = Path.GetFileName(selectedPath);
                string projectImagesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");

                if (!Directory.Exists(projectImagesDir))
                    Directory.CreateDirectory(projectImagesDir);

                string destPath = Path.Combine(projectImagesDir, selectedImageFileName);

                try
                {
                    if (!File.Exists(destPath))
                        File.Copy(selectedPath, destPath);

                    imgCover.Source = new BitmapImage(new Uri(destPath, UriKind.Absolute));
                    MessageBox.Show("Изображение выбрано:\n" + selectedImageFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при копировании изображения: " + ex.Message);
                }
            }
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Book.Title) ||
                string.IsNullOrWhiteSpace(tbAuthor.Text) ||
                Book.Price <= 0 || Book.Stock < 0)
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля, включая автора.");
                return;
            }

            var context = AppConnect.BookstoreModel;

            Book.GenreID = (int?)cbGenres.SelectedValue;
            Book.PublisherID = (int?)cbPublishers.SelectedValue;
            Book.AgeRatingID = (int?)cbAgeRatings.SelectedValue;

            string authorName = tbAuthor.Text.Trim();
            var author = context.Authors.FirstOrDefault(a => a.FullName == authorName);
            if (author == null)
            {
                author = new Authors { FullName = authorName };
                context.Authors.Add(author);
                context.SaveChanges();
            }
            Book.AuthorID = author.AuthorID;

            Book.SeriesID = null;
            string seriesName = tbSeriesName.Text.Trim();
            if (!string.IsNullOrWhiteSpace(seriesName))
            {
                var series = context.BookSeries.FirstOrDefault(s => s.SeriesName.ToLower() == seriesName.ToLower());
                if (series == null)
                {
                    series = new BookSeries { SeriesName = seriesName };
                    context.BookSeries.Add(series);
                    context.SaveChanges();
                }
                Book.SeriesID = series.SeriesID;
            }

            if (int.TryParse(tbSeriesOrder.Text, out int order))
                Book.SeriesOrder = order;
            else
                Book.SeriesOrder = null;

            if (!string.IsNullOrEmpty(selectedImageFileName))
            {
                var existingImage = context.BookImage.FirstOrDefault(i => i.ImagePath == selectedImageFileName);
                if (existingImage == null)
                {
                    existingImage = new BookImage
                    {
                        ImagePath = selectedImageFileName
                    };
                    context.BookImage.Add(existingImage);
                    context.SaveChanges();
                }
                Book.BookImageID = existingImage.ImageID;
            }

            Book.Description = tbDescription.Text.Trim();

            if (Book.BookID == 0)
                context.Books.Add(Book);

            context.SaveChanges();
            MessageBox.Show("Книга успешно сохранена!");
            NavigationService.Navigate(new PageProducts());
        }
    }
}