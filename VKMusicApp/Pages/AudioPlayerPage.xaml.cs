using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class AudioPlayerPage : ContentPage
{
	private uint animationDuraction = 100;

    public AudioPlayerPage(AudioPlayerViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;

		vm.Player = mediaElement;
    }

    private void HideMusicQueen(object sender, SwipedEventArgs e)
    {
        _ = Player.TranslateTo(0, 0, animationDuraction, Easing.Default);
    }

	private void ShowMusicQueen(object sender, SwipedEventArgs e)
	{
		_ = Player.TranslateTo(-this.Width, 0, animationDuraction, Easing.Default);
    }
}