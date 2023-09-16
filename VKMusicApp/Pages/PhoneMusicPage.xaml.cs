using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class PhoneMusicPage : ContentPage
{
	public PhoneMusicPage(PhoneMusicViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}