using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel viewModel;

	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();
		
        viewModel = vm;
		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (viewModel.IsLogin().Result)
        {
            Shell.Current.GoToAsync(nameof(AccountMusicPage));
        }
    }
}