using SharedModels;
using static Android.Hardware.Camera;

namespace Varastokirjanpito_projekti.Pages
{
    public partial class BookSold : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Books _book;
        private readonly Users _user;

        public BookSold(Books book, Users user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _book = book;
            _user = user;
            BindingContext = _book;
        }

        private async void SellBook(object sender, EventArgs e)
        {
            //create timestamp
            DateTime now = DateTime.Now;
            string timestamp = now.ToString("dd.MM.yyyy HH:mm");

            try
            {
                int SellCount = int.Parse(SoldCount.Text);


                if (_book.Count > SellCount) // if there are more books on stack than the user sells, use the PUT method
                {
                    _book.Count -= SellCount; 
                    await _apiService.PutBooksControllerDataAsync(_book.Id, _book);
                    await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"Myyty {SellCount} kpl, {timestamp}", $"{Notes.Text}");
                    await DisplayAlert("", "Myyty.", "OK");
                    await Navigation.PushAsync(new Products(_user));
                }
                else if (_book.Count < SellCount) // if there are less books on stack than the user tries to sell, wont't work
                {
                    await DisplayAlert("", "Kirjoja ei ole varastossa tarpeeksi.", "OK");
                }
                else //if the amount user want's to sell is the same as the count, use the DELETE method
                {
                    var response = await _apiService.DeleteBooksControllerDataAsync(_book.Id);
                    if (response != null)
                    {
                        await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"Myyty {SellCount} kpl, {timestamp}", $"{Notes.Text}");
                        await DisplayAlert("", "Myyty.", "OK");
                        await Navigation.PushAsync(new Products(_user));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Kirjan myynti ep�onnistui.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Varmista ett� m��r� on kokonaisluku.", "OK");
            }
        }
    }
}