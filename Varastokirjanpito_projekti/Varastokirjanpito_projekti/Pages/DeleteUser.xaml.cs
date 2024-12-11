using System.Collections.ObjectModel;
using SharedModels;

namespace Varastokirjanpito_projekti.Pages;

public partial class DeleteUser : ContentPage
{
    private readonly ApiService _apiService;
    public ObservableCollection<Users> UsersList { get; set; }
    public DeleteUser()
	{
		InitializeComponent();
        _apiService = new ApiService();
        UsersList = new ObservableCollection<Users>();
        UsersListView.ItemsSource = UsersList;
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
            InfoLabel.Text = "Klikkaa valitsemaasi käyttäjää nähdäksesi kaikki tiedot.";
        }
        catch
        (Exception ex)
        {
            InfoLabel.Text = $"Error: {ex.Message}";
        }
    }
    private async void OnUserTapped(object sender, EventArgs e)
    { 
        var user = (Users)((TappedEventArgs)e).Parameter;
        bool answer = await DisplayAlert("Poista käyttäjä", $"Haluatko poistaa käyttäjän {user.Username}?", "Kyllä", "Peruuta"); 
        if (answer) 
        {
            try 
            {
                var deletedUser = await _apiService.DeleteGetUsersControllerDataByUsernameAsync(user.Username);
                if (deletedUser != null) 
                {
                    UsersList.Remove(user);
                    InfoLabel.Text = $"Käyttäjä {user.Username} poistettu."; 
                }
                else { InfoLabel.Text = "Käyttäjää ei löytynyt tai poistaminen epäonnistui."; 
                }
            }
            catch (Exception ex) 
            {
                InfoLabel.Text = $"Error: {ex.Message}"; 
            }
        }
    }
}