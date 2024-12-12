using SharedModels;

namespace Varastokirjanpito_projekti.Pages;

public partial class UserControl : ContentPage

{
	private readonly Users _user;
	public UserControl(Users user)
	{
		InitializeComponent();
		_user = user;
	}

    private async void AddUser(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new CreateUser(_user));
	}

    private async void DeleteUser(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeleteUser(_user));
    }

    private async void BackToMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminMenu(_user));
    }
}