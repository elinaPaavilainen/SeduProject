using Newtonsoft.Json;
using SharedModels;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Varastokirjanpito_projekti.Pages;

public partial class CreateUser : ContentPage
{
    private readonly ApiService _apiService;
    public ObservableCollection<Users> UsersList { get; set; }
    public CreateUser()
    {
        InitializeComponent();
        _apiService = new ApiService();
        UsersList = new ObservableCollection<Users>();
        UsersListView.ItemsSource = UsersList;
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
            await DisplayAlert("", "Uusi k‰ytt‰j‰ lis‰tty!", "OK");
            NewUsername.Text = "";
            NewUserPassword.Text = "";
        }

        catch (Exception ex) 
        {
            await DisplayAlert("Error", $"Failed to save data: {ex.Message}", "OK"); 
        }
    }
    private async void SearchUserClicked(object sender, EventArgs e)
    {
        try
        {
            string UserName = SearchUser.Text;
            IEnumerable<Users> result;

            if (string.IsNullOrEmpty(UserName))
            {
                result = await _apiService.GetUsersControllerDataAsync();
            }
            else
            {
                result = await _apiService.SearchUsersAsync(UserName);
            }
            UsersList.Clear();
            foreach (var user in result)
            {
                UsersList.Add(user);
            }
            InfoLabel.Text = "Klikkaa valitsemaasi k‰ytt‰j‰‰ n‰hd‰ksesi kaikki tiedot.";
        }
        catch
        (Exception ex)
        {
            InfoLabel.Text = $"Error: {ex.Message}";
        }
    }
    private async void OnUserTapped(object sender, EventArgs e)
    {
        InfoLabel.Text = "Clicked";
    }
}