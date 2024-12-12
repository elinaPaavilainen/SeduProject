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

        try 
        {     
        var data = new Books
        {
            Author = Author.Text,
            Title = Title.Text,
            Count = int.Parse(ProductCount.Text),
            Price = float.Parse(ProductPrice.Text.Replace(',', '.'), CultureInfo.InvariantCulture)
        };
            try
            {
                var result = _apiService.PostBooksControllerDataAsync(data);
                await _apiService.LogDeletionAsync(_user.Username, $"{Author.Text}: {Title.Text}", "Lis‰tty", AdditionalInfo.Text);
                await DisplayAlert("", "Kirja lis‰tty.", "OK");

                Author.Text = "";
                Title.Text = "";
                ProductCount.Text = "";
                ProductPrice.Text = "";
                AdditionalInfo.Text = "";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save data: {ex.Message}", "OK");
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