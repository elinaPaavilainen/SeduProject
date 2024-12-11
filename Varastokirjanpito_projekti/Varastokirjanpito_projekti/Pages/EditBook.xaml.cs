using SharedModels;

namespace Varastokirjanpito_projekti.Pages
{
    public partial class EditBook : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Books _book;
        private readonly Users _user;

        public EditBook(Books book, Users user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _book = book;
            _user = user;
            BindingContext = _book;
        }

        private async void SaveChanges(object sender, EventArgs e)
        {
            try
            {
                var response = await _apiService.PutBooksControllerDataAsync(_book.Id, _book);
                if (response != null)
                {
                    await DisplayAlert("", "Kirjan tiedot päivitetty.", "OK");
                    await _apiService.LogDeletionAsync(_user.Username, $"{ _book.Author}: {_book.Title}", "Muokattu", AdditionalInfo.Text);
                    await Navigation.PushAsync(new Products(_user));
                }
                else
                {
                    await DisplayAlert("Error", "Failed to update book details", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }

    }
}