using System.Collections.ObjectModel;
using SharedModels;

namespace Varastokirjanpito_projekti.Pages;

public partial class DeleteUser : ContentPage
{
    private readonly ApiService _apiService;
    private readonly Users _user;
    public ObservableCollection<Users> UsersList { get; set; }
    public DeleteUser(Users user)
	{
		InitializeComponent();
        _apiService = new ApiService();
        _user = user;
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
            InfoLabel.Text = "Klikkaa valitsemaasi k�ytt�j�� poistaaksesi.";
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
                bool isDeleted = await _apiService.DeleteUsersControllerDataAsync(user.Username);
                if (isDeleted) 
                {
                    UsersList.Remove(user); 
                    InfoLabel.Text = $"K�ytt�j� {user.Username} poistettu."; 
                }
                else 
                {
                    InfoLabel.Text = "K�ytt�j�� ei l�ytynyt tai poistaminen ep�onnistui.";
                }
            } 
            catch (Exception ex) 
            {
                InfoLabel.Text = $"Error: {ex.Message}"; } 
        }
    }
    private async void BackToMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminMenu(_user));
    }
}