using SharedModels;
using System.Collections.ObjectModel;

namespace Varastokirjanpito_projekti.Pages;

public partial class CreateUser : ContentPage
{
    private readonly ApiService _apiService;
    private readonly Users _user;
    public CreateUser(Users user)
    {
        InitializeComponent();
        _apiService = new ApiService();
        _user = user;
    }
    private async void CreateNewUser(object sender, EventArgs e)
    {
        var UserInfo = new Users
        {
            Username = NewUsername.Text,
            Password = NewUserPassword.Text,
            Admin = false
        };

        try
        {
            var result = _apiService.PostUsersControllerDataAsync(UserInfo);
            await DisplayAlert("", "Uusi käyttäjä lisätty!", "OK");
            NewUsername.Text = "";
            NewUserPassword.Text = "";
        }

        catch (Exception ex) 
        {
            await DisplayAlert("Error", $"Failed to save data: {ex.Message}", "OK"); 
        }
    }
    private async void BackToMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminMenu(_user));
       
    }
}