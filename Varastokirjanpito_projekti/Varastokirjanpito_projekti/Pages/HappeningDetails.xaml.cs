using SharedModels;

namespace Varastokirjanpito_projekti.Pages;

public partial class HappeningDetails : ContentPage
{
    private readonly Users _user;
    public HappeningDetails(Deleted_books happening, Users user) 
    {
        InitializeComponent();
        BindingContext = happening;
        _user = user; 
    }
    private async void BackToMenu(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AdminMenu(_user));
    }
}