﻿using Newtonsoft.Json;
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
                if (Username.Text == "" || Password.Text == "")
                {
                    InfoLabel.Text = "Kirjoita käyttäjätunnus ja salasana";
                }
                
                else
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
                                var navigationStack = Navigation.NavigationStack;
                                var loginPage = navigationStack.FirstOrDefault(page => page is LoginPage);
                                if (loginPage != null)
                                {   // if non-admin users signs in, they can't go back to the login page
                                    Navigation.RemovePage(loginPage);
                                }
                            }
                        }
                        else
                        {
                            InfoLabel.Text = "Käyttäjätunnus ja salasana eivät täsmää.";
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                InfoLabel.Text = "Yhteysongelma"; 
            }       
        }
    }
}
    