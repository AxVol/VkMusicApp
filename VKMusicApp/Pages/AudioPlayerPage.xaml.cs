using CommunityToolkit.Maui.Core.Primitives;
using VKMusicApp.ViewModels;

namespace VKMusicApp.Pages;

public partial class AudioPlayerPage : ContentPage
{
	public AudioPlayerPage(AudioPlayerViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        mediaElement.MediaFailed += ErrorHandler;
    }

	public void OnClicked(object sender, EventArgs e)
	{
		mediaElement.Play();
    }

	private void ErrorHandler(object sender, MediaFailedEventArgs e)
	{
		label1.Text = e.ErrorMessage;
	}
}