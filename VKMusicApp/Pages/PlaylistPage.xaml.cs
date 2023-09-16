using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class PlaylistPage : ContentPage
{
	public PlaylistPage(PlaylistViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}