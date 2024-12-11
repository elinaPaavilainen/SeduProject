namespace Varastokirjanpito_projekti.Pages;

public partial class UserControl : ContentPage
{
	public UserControl()
	{
		InitializeComponent();
	}

    private async void AddUser(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new CreateUser());
	}

    private async void DeleteUser(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeleteUser());
    }
}