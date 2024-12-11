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
            InfoLabel.Text = "Klikkaa valitsemaasi k�ytt�j�� n�hd�ksesi kaikki tiedot.";
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
        bool answer = await DisplayAlert("Poista k�ytt�j�", $"Haluatko poistaa k�ytt�j�n {user.Username}?", "Kyll�", "Peruuta"); 
        if (answer) 
        {
            try 
            {
                var deletedUser = await _apiService.DeleteGetUsersControllerDataByUsernameAsync(user.Username);
                if (deletedUser != null) 
                {
                    UsersList.Remove(user);
                    InfoLabel.Text = $"K�ytt�j� {user.Username} poistettu."; 
                }
                else { InfoLabel.Text = "K�ytt�j�� ei l�ytynyt tai poistaminen ep�onnistui."; 
                }
            }
            catch (Exception ex) 
            {
                InfoLabel.Text = $"Error: {ex.Message}"; 
            }
        }
    }
}