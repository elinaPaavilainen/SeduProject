using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using SharedModels;
using System.Collections.ObjectModel;

namespace Varastokirjanpito_projekti.Pages;

public partial class ViewHappenings : ContentPage
{
    private readonly ApiService _apiService;
    public ObservableCollection<Deleted_books> Deleted_booksList { get; set; }
    public ViewHappenings()
    {
        InitializeComponent();
        _apiService = new ApiService();
        Deleted_booksList = new ObservableCollection<Deleted_books>();
        Deleted_booksListView.ItemsSource = Deleted_booksList;
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
            foreach (var deleted_book in result)
            {
                Deleted_booksList.Add(deleted_book);
            }
            FoundHappenings.Text = "Klikkaa valitsemaasi tapahtumaa nähdäksesi kaikki tiedot.";
        }
        catch
        (Exception ex)
        {
            FoundHappenings.Text = $"Error: {ex.Message}";
        }
    }
    private async void OnHappeningTapped(object sender, EventArgs e)
    {
        var stackLayout = sender as StackLayout;
        var happening = stackLayout.BindingContext as Deleted_books;
        if (happening != null)
        {
            await Navigation.PushAsync(new HappeningDetails(happening));
        }
    }
}