using SharedModels;
using System.Globalization;

namespace Varastokirjanpito_projekti.Pages;

public partial class AddProduct : ContentPage
{
    private readonly ApiService _apiService;
    private readonly Users _user;
    public AddProduct(Users user)
	{
		InitializeComponent();
        _apiService = new ApiService();
        _user = user;
	}
    private async void ProductAdded(object sender, EventArgs e)
    {
        //Create timestamp
        DateTime now = DateTime.Now;
        string timestamp = now.ToString("dd.MM.yyyy HH:mm");
        try 
        {     
        var data = new Books
        {
            Author = Author.Text,
            Title = Title.Text,
            Count = int.Parse(ProductCount.Text),
            Price = float.Parse(ProductPrice.Text.Replace(',', '.'), CultureInfo.InvariantCulture) //doesn't matter if user uses . or , in price
        };
            try
            {
                var result = _apiService.PostBooksControllerDataAsync(data);
                await _apiService.LogDeletionAsync(_user.Username, $"{Author.Text}: {Title.Text}", $"Lis‰tty, {timestamp}", AdditionalInfo.Text); // Log the adding to database table
                await DisplayAlert("", "Kirja lis‰tty.", "OK");

                Author.Text = "";
                Title.Text = "";
                ProductCount.Text = "";
                ProductPrice.Text = "";
                AdditionalInfo.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("", $"Tallennus ep‰onnistui: {ex.Message}", "OK");
            }
        }
        catch
        {
            await DisplayAlert("T‰yt‰ pakolliset kent‰t", "Huomioi ett‰ m‰‰r‰n t‰ytyy olla kokonaisluku ja hinnan kokonais- tai desimaaliluku.", "OK");
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