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
        if (NewUserPassword.Text == NewUserPasswordAgain.Text)
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
                await DisplayAlert("", "Uusi k‰ytt‰j‰ lis‰tty!", "OK");
                NewUsername.Text = "";
                NewUserPassword.Text = "";
                NewUserPasswordAgain.Text = "";
            }

            catch (Exception ex) 
            {
                await DisplayAlert("Error", $"Failed to save data: {ex.Message}", "OK"); 
            }
        }
        else
        {
            await DisplayAlert("Tarkista salasana", "Salasanat eiv‰t t‰sm‰‰.", "OK");
        }
    }
    private async void BackToMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminMenu(_user));
       
    }
}