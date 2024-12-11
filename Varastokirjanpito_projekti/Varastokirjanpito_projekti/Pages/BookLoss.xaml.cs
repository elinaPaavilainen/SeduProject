using Microsoft.Maui.Controls;
using SharedModels;
using System.Globalization;

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
            try
            {

                if (_book.Count > 1)
                {
                    _book.Count -= 1;
                    await _apiService.PutBooksControllerDataAsync(_book.Id, _book);
                    await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", "Hävikki", $"{Notes.Text}");
                    await DisplayAlert("", "Kirja poistettu.", "OK");
                    await Navigation.PushAsync(new Products(_user));
                }

                else
                {
                    var response = await _apiService.DeleteBooksControllerDataAsync(_book.Id);
                    if (response != null)
                    {
                        //LogDeletionAsync(int id, string deletedBy, string authorAndTitle, string lossOrSold, string notes)
                        await _apiService.LogDeletionAsync(_user.Username, $"{_book.Author}: {_book.Title}", "Loss", $"{Notes.Text}");
                        await DisplayAlert("", "Kirja poistettu.", "OK");
                        await Navigation.PushAsync(new Products(_user));
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to delete book", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}