using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class SearchMusicPage : ContentPage
{
	public SearchMusicPage(SearchMusicViewModel vm)
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