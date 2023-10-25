using System.Collections.ObjectModel;
using VKMusicApp.ViewModels;
using VkNet.Model;

namespace VKMusicApp.Pages;

public partial class MusicPlaylistPage : ContentPage
{
	public MusicPlaylistPage(MusicPlaylistViewModel vm)
	{
		InitializeComponent();

        BindingContext = vm;
    }
}