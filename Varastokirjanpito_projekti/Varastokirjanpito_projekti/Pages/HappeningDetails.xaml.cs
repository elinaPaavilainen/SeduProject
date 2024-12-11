using SharedModels;

namespace Varastokirjanpito_projekti.Pages;

public partial class HappeningDetails : ContentPage
{
    public HappeningDetails(Deleted_books happening) 
    {
        InitializeComponent(); 
        BindingContext = happening; 
    }
}