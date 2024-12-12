using SharedModels;
using System.Text.RegularExpressions;

namespace Varastokirjanpito_projekti.Pages
{
    public partial class ChangePassword : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly Users _user;
        public ChangePassword(Users user)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _user = user;
        }
        private async void SaveNewPassword(object sender, EventArgs e)
        {
            try
            {
                string OldPasswordInput = OldPassword.Text;
                string NewPasswordInput = NewPassword.Text; 
                string NewPasswordInputAgain = NewPasswordAgain.Text;


                if (NewPasswordInput.Length < 8 || !Regex.IsMatch(NewPasswordInput, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W_]).+$"))
                {
                    InfoLabel.Text = "Salasana täytyy olla vähintään 8 merkkiä pitkä ja sisältää vähintään yhden ison kirjaimen, pienen kirjaimen, numeron ja erikoismerkin.";
                }
                else
                {

                    if (OldPasswordInput == _user.Password && NewPasswordInput == NewPasswordInputAgain)
                    {
                        _user.Password = NewPasswordInput;
                        var result = await _apiService.PutUsersControllerDataAsync(_user.Id, _user);

                        if (result != null)
                        {
                            await DisplayAlert("", "Salasana vaihdettu.", "OK");
                            if (_user.Admin == true)
                            {
                                await Navigation.PushAsync(new AdminMenu(_user));
                            }
                            else
                            {
                                await Navigation.PushAsync(new UserMenu(_user));
                            }

                        }
                        else
                        {
                            await DisplayAlert("", "Salasanan vaihto epäonnistui.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("", "Salasanat eivät täsmää.", "OK");
                    }
                }
            } 
            catch (Exception ex) 
            {
                await DisplayAlert("Error", $"Virhe: {ex.Message}", "OK"); 
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
}