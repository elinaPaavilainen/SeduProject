using SharedModels;

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
            DateTime now = DateTime.Now;
            string timestamp = now.ToString("dd.MM.yyyy HH:mm");

            try
            {
                int SellCount = int.Parse(SoldCount.Text);


                if (_book.Count > SellCount) 
                {
                    _book.Count -= SellCount; 
                    await _apiService.PutBooksControllerDataAsync(_book.Id, _book);
                    await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"Myyty {SellCount} kpl, {timestamp}", $"{Notes.Text}");
                    await DisplayAlert("", "Myyty.", "OK");
                    await Navigation.PushAsync(new Products(_user));
                }
                else if (_book.Count < SellCount)
                {
                    await DisplayAlert("", "Kirjoja ei ole varastossa tarpeeksi.", "OK");
                }
                else
                {
                    var response = await _apiService.DeleteBooksControllerDataAsync(_book.Id);
                    if (response != null)
                    {
                        //LogDeletionAsync(int id, string deletedBy, string authorAndTitle, string lossOrSold, string notes)
                        await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"Myyty {SellCount} kpl, {timestamp}", $"{Notes.Text}");
                        await DisplayAlert("", "Myyty.", "OK");
                        await Navigation.PushAsync(new Products(_user));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Kirjan myynti epäonnistui.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Varmista että määrä on kokonaisluku.", "OK");
            }
        }
    }
}