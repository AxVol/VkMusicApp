using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		
		BindingContext = vm;
    }
}