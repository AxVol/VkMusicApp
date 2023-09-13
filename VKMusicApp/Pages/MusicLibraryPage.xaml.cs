using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class MusicLibraryPage : ContentPage
{
	public MusicLibraryPage(MusicLibraryViewModel vm)
	{
        InitializeComponent();

        BindingContext = vm;

        vm.EntryFocus += EntryFocus;
	}

	private void EntryFocus()
	{
		Search.Focus();
    }
}