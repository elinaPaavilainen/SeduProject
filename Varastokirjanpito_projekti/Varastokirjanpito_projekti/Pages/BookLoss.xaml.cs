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
            //create timestamp
            DateTime now = DateTime.Now;
            string timestamp = now.ToString("dd.MM.yyyy HH:mm");

            if (Notes.Text != "")
            {
                try
                {
                    int LoosedCount = int.Parse(LossCount.Text);

                    if (_book.Count > LoosedCount) // if there are more books on stack than the user deletes, use the PUT method
                    {
                        _book.Count -= LoosedCount;
                        await _apiService.PutBooksControllerDataAsync(_book.Id, _book);
                        await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"H�vikkiin {LoosedCount} kpl, {timestamp}", $"{Notes.Text}");
                        await DisplayAlert("", "Poistettu.", "OK");
                        Notes.Text = "";
                        await Navigation.PushAsync(new Products(_user));
                    }

                    else if (_book.Count < LoosedCount) // if there are less books on stack than the user tries to delete, wont't work
                    {
                        await DisplayAlert("", "Kirjoja ei ole varastossa tarpeeksi.", "OK");
                    }

                    else    //if the amount user want's to delete is the same as the count, use the DELETE method
                    {
                        var response = await _apiService.DeleteBooksControllerDataAsync(_book.Id);
                        if (response != null)
                        {
                            await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", $"H�vikkiin {LoosedCount} kpl, {timestamp} ", $"{Notes.Text}");
                            await DisplayAlert("", "Kirja poistettu.", "OK");
                            Notes.Text = "";
                            await Navigation.PushAsync(new Products(_user));
                        }
                        else
                        {
                            await DisplayAlert("Error", "Kirjan poisto ep�onnistui.", "OK");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", "Varmista ett� m��r� on kokonaisluku.", "OK");
                }
            }
            else
            {
                await DisplayAlert("", "T�yt� poiston lis�tiedot", "OK");
            }
        }
       
    }
}