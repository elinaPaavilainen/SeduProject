using SharedModels;

namespace Varastokirjanpito_projekti.Pages
{
    public partial class BookLoss : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Books _book;
        private readonly Users _user;

        public BookLoss(Books book, Users user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _book = book;
            _user = user;
            BindingContext = _book;
        }

        private async void DeleteBook(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string timestamp = now.ToString("dd.MM.yyyy HH:mm");

            if (Notes.Text != "")
            {
                try
                {
                    int LoosedCount = int.Parse(LossCount.Text);

                    if (_book.Count > LoosedCount)
                    {
                        _book.Count -= LoosedCount;
                        await _apiService.PutBooksControllerDataAsync(_book.Id, _book);
                        await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"Hävikkiin {LoosedCount} kpl, {timestamp}", $"{Notes.Text}");
                        await DisplayAlert("", "Poistettu.", "OK");
                        Notes.Text = "";
                        await Navigation.PushAsync(new Products(_user));
                    }

                    else if (_book.Count < LoosedCount)
                    {
                        await DisplayAlert("", "Kirjoja ei ole varastossa tarpeeksi.", "OK");
                    }

                    else
                    {
                        var response = await _apiService.DeleteBooksControllerDataAsync(_book.Id);
                        if (response != null)
                        {
                            //LogDeletionAsync(int id, string deletedBy, string authorAndTitle, string lossOrSold, string notes)
                            await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"Hävikkiin {LoosedCount} kpl, {timestamp} ", $"{Notes.Text}");
                            await DisplayAlert("", "Kirja poistettu.", "OK");
                            Notes.Text = "";
                            await Navigation.PushAsync(new Products(_user));
                        }
                        else
                        {
                            await DisplayAlert("Error", "Kirjan poisto epäonnistui.", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Varmista että määrä on kokonaisluku.", "OK");
                }
            }
            else
            {
                await DisplayAlert("", "Täytä poiston lisätiedot", "OK");
            }
        }
       
    }
}