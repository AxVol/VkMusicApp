using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class AccountMusicPage : ContentPage
{
	public AccountMusicPage(AccountMusicViewModel vm)
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