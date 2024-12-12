using Newtonsoft.Json;
using SharedModels;

namespace Varastokirjanpito_projekti.Pages
{
    public partial class LoginPage : ContentPage
    {
        private readonly ApiService _apiService;
        public LoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }       
        private async void LoginClicked(object sender, EventArgs e)
        {
            try
            {
                string UserNameInput = Username.Text;
                string PasswordInput = Password.Text;
                var data = await _apiService.LoginGetUsersControllerDataByUsernameAsync(UserNameInput);
                // use the data user is the class, and user.something is the object
                var user = JsonConvert.DeserializeObject<Users>(data);
                if (user != null)
                {
                    if (PasswordInput == user.Password)
                    {
                        if (user.Admin == true)
                        {
                            await Navigation.PushAsync(new AdminMenu(user));
                            Username.Text = "";
                            Password.Text = "";
                        }
                        else
                        {
                            await Navigation.PushAsync(new UserMenu(user));
                            Username.Text = "";
                            Password.Text = "";
                        }
                    }
                    else
                    {
                        Tulos.Text = "Käyttäjätunnus ja salasana eivät täsmää.";
                    }
                }
            }
            catch (Exception ex) 
            {
                Tulos.Text = ex.Message; 
            }       
        }
    }
}
    