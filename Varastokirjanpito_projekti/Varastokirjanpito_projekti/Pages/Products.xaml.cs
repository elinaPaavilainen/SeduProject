using SharedModels;
using System.Collections.ObjectModel;

namespace Varastokirjanpito_projekti.Pages;

public partial class Products : ContentPage
{
	private readonly ApiService _apiService;
	private readonly Users _user;
    public ObservableCollection<Books> BooksList { get; set; }
    public Products(Users user)
	{
		InitializeComponent();
		_apiService = new ApiService();
        BooksList = new ObservableCollection<Books>(); 
		BooksListView.ItemsSource = BooksList;
		_user = user;
    }
    private async void SearchButtonClicked(object sender, EventArgs e)
    {
        try
        {
            string query = Search.Text; 
			IEnumerable<Books> result; 

			if (string.IsNullOrEmpty(query))
            {
                result = await _apiService.GetBooksControllerDataAsync();
				// Call a method to get all books
			}
			else 
			{
				result = await _apiService.SearchBooksAsync(query);
			}
			BooksList.Clear();
			foreach (var book in result.OrderBy(b => b.Author)) //Organize books in alphabetical order by author name
			{
				BooksList.Add(book);
			}
			Result.Text = "Klikkaa valitsemaasi kirjaa nähdäksesi kaikki tiedot ja toimintavaihtoehdot.";
		}
		catch
		(Exception ex) 
		{
			Result.Text = $"Odottamaton virhe: {ex.Message}"; 
		}
	}
    private async void OnBookTapped(object sender, EventArgs e) 
	{
		var stackLayout = sender as StackLayout; 
		var book = stackLayout.BindingContext as Books; 
		if (book != null) 
		{
			await Navigation.PushAsync(new BookDetails(book, _user)); 
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