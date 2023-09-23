using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class AudioPlayerPage : ContentPage
{
	public AudioPlayerPage(AudioPlayerViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}