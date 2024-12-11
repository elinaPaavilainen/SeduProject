using Microsoft.Maui.Controls;
using Varastokirjanpito_projekti.Pages;

namespace Varastokirjanpito_projekti
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        { 
            var LoginWindow = new Window(new NavigationPage(new LoginPage())); 
            return LoginWindow; 
        }
    }
}