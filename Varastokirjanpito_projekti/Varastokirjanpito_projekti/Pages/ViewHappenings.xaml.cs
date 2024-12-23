using SharedModels;
using System.Collections.ObjectModel;

namespace Varastokirjanpito_projekti.Pages;

public partial class ViewHappenings : ContentPage
{
    private readonly ApiService _apiService;
    private readonly Users _user;
    public ObservableCollection<Deleted_books> Deleted_booksList { get; set; }
    public ViewHappenings(Users user)
    {
        InitializeComponent();
        _apiService = new ApiService();
        Deleted_booksList = new ObservableCollection<Deleted_books>();
        Deleted_booksListView.ItemsSource = Deleted_booksList;
        _user = user;   
    }
    private async void SearchButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string query = SearchHappenings.Text;
            IEnumerable<Deleted_books> result;

            if (string.IsNullOrEmpty(query))
            {
                result = await _apiService.GetDeletedBooksDataAsync();
                // Call a method to get all books
            }
            else
            {
                result = await _apiService.SearchDeletedBooksAsync(query);
            }
            Deleted_booksList.Clear();
            foreach (var deleted_book in result.Reverse())
            {
                Deleted_booksList.Add(deleted_book);
            }
            FoundHappenings.Text = "Klikkaa valitsemaasi tapahtumaa n�hd�ksesi kaikki tiedot.";
        }
        catch
        (Exception ex)
        {
            await DisplayAlert("", "Haullasi ei l�ytynyt mit��n, yrit� toista hakusanaa", "OK");
        }
    }
    private async void OnHappeningTapped(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        var happening = stackLayout.BindingContext as Deleted_books;
        if (happening != null)
        {
            await Navigation.PushAsync(new HappeningDetails(happening, _user));
        }
    }
    private async void BackToMenu(object sender, EventArgs e)
    {
        if (_user.Admin == true)
        {
            await Navigation.PushAsync(new AdminMenu(_user));
        }
        else
        {
            await Navigation.PushAsync(new UserMenu(_user));
        }
    }
}