using SharedModels;

namespace Varastokirjanpito_projekti.Pages; 
public partial class BookDetails : ContentPage
{ 
    private readonly ApiService _apiService;  
    private readonly Users _user;
    public BookDetails(Books book, Users user) 
    {
        InitializeComponent();
        _apiService = new ApiService();
        _user = user;
        BindingContext = book; 
    }

    private async void Modify(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EditBook((Books)BindingContext, _user));
    }
    private async void Loss(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BookLoss((Books)BindingContext, _user));
    }
    private async void Sold(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BookSold((Books)BindingContext, _user));
    } 
}