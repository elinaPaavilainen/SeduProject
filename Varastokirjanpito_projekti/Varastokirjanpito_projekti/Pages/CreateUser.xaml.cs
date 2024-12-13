using SharedModels;
using System.Text.RegularExpressions;

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
        // Password requirements check
        if (NewUserPassword.Text.Length < 8 || !Regex.IsMatch(NewUserPassword.Text, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W_]).+$"))
        {
            await DisplayAlert("", "Salasana t�ytyy olla v�hint��n 8 merkki� pitk� ja sis�lt�� v�hint��n yhden ison kirjaimen, pienen kirjaimen, numeron ja erikoismerkin.", "OK");
        }
        else
        {
            if (NewUserPassword.Text == NewUserPasswordAgain.Text)
            {
                var UserInfo = new Users
                {
                    Username = NewUsername.Text,
                    Password = NewUserPassword.Text,
                    Admin = false //Admin is automaticly false, maybe in future this could be a thing the admin can choose
                };

                try
                {
                    var result = _apiService.PostUsersControllerDataAsync(UserInfo);
                    await DisplayAlert("", "Uusi k�ytt�j� lis�tty!", "OK");
                    NewUsername.Text = "";
                    NewUserPassword.Text = "";
                    NewUserPasswordAgain.Text = "";
                }

                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"K�ytt�j�n lis��minen ep�onnistui: {ex.Message}", "OK");
                }
            }

            else
            {
                await DisplayAlert("Tarkista salasana", "Salasanat eiv�t t�sm��.", "OK");
            }
        }
    }
    private async void BackToMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminMenu(_user));
       
    }
}