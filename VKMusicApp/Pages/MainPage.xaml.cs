using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
        InitializeComponent();

        BindingContext = new MainPageViewModel();
	}
}

