using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class MusicPlaylistPage : ContentPage
{
	public MusicPlaylistPage(MusicPlaylistViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}