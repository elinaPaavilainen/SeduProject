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

                // Password requirements
                if (NewPasswordInput.Length < 8 || !Regex.IsMatch(NewPasswordInput, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[\W_]).+$"))
                {
                    await DisplayAlert("", "Salasana t�ytyy olla v�hint��n 8 merkki� pitk� ja sis�lt�� v�hint��n yhden ison kirjaimen, pienen kirjaimen, numeron ja erikoismerkin.", "OK");
                }
                else if (OldPasswordInput == NewPasswordInput) //New can't be the same as the old
                {
                    await DisplayAlert("", "Uusi salasana t�ytyy olla eri kuin vanha.", "OK");
                }

                else
                {
                    // Check if all the requirements are met
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
                                OldPassword.Text = "";
                                NewPassword.Text = "";
                                NewPasswordAgain.Text = "";
                            }
                            else
                            {
                                await Navigation.PushAsync(new UserMenu(_user));
                                OldPassword.Text = "";
                                NewPassword.Text = "";
                                NewPasswordAgain.Text = "";
                            }

                        }
                        else
                        {
                            await DisplayAlert("", "Salasanan vaihto ep�onnistui.", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("", "Salasanat eiv�t t�sm��.", "OK");
                    }
                }
            } 
            catch (Exception ex) 
            {
                await DisplayAlert("T�yt� kaikki kent�t", "", "OK"); 
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