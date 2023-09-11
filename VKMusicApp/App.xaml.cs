using VKMusicApp.Pages;

namespace VKMusicApp;

public partial class App : Application
{
	public App(LoginPage page)
	{
		InitializeComponent();

		MainPage = new NavigationPage(page);
	}
}
