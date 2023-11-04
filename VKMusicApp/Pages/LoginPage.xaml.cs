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
        Loaded += LoginPage_Loaded;
    }

    private async void LoginPage_Loaded(object sender, EventArgs e)
    {
        if (viewModel.Exception == "2FA")
        {
            string tf = await DisplayPromptAsync("Двухфакторная авторизация", "Код", "Войти", "Отмена");

            if (await viewModel.LoginWithTFAsync(viewModel.Login, viewModel.Password, tf))
                await Shell.Current.GoToAsync(nameof(AccountMusicPage));
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (LoginViewModel.HasEthernet())
        {
            if (viewModel.IsLogin().Result)
            {
                await Shell.Current.GoToAsync(nameof(AccountMusicPage));
            }
        }
        else
        {
            await Shell.Current.GoToAsync(nameof(PhoneMusicPage));
        }
    }
}