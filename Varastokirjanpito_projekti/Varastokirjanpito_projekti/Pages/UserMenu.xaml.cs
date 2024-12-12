using SharedModels;

namespace Varastokirjanpito_projekti.Pages;

public partial class UserMenu : ContentPage
{
    private readonly Users _user;
    public UserMenu(Users user)
    {
        InitializeComponent();
        _user = user;
        BindingContext = _user;
    }

    private async void AddProduct(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddProduct(_user));
    }

    private async void GetProducts(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Products(_user));
    }

    private async void ChangePassword(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePassword(_user));
    }
    private async void ExitButtonClicked(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }
}