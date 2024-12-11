using SharedModels;

namespace Varastokirjanpito_projekti.Pages;

public partial class AdminMenu : ContentPage
{
    private readonly Users _user;
	public AdminMenu(Users user)
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

    private async void ControlUsers(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UserControl());
    }

    private async void ChangePassword(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePassword(_user));
    }

    private async void ViewHappenings(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ViewHappenings());
    }
}