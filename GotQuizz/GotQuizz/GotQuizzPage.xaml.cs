using GotQuizz.ViewModels;
using Xamarin.Forms;

namespace GotQuizz
{
	public partial class GotQuizzPage : ContentPage
	{
	    private GotQuizzViewModel _vm = new GotQuizzViewModel();

	    public GotQuizzPage()
	    {
	        InitializeComponent();
	        BindingContext = _vm;
	    }

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();

	        _vm.Start();
	    }
	}
}
