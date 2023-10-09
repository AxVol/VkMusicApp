using VKMusicApp.ViewModels;
using VkNet.Model;

namespace VKMusicApp.Pages;

public partial class AudioPlayerPage : ContentPage
{
	private uint animationDuraction = 400;

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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Grid grid = sender as Grid;
        string title = string.Empty;
        string artist = string.Empty;
        
        foreach (IView obj in grid.Children)
        {
            if (obj.AutomationId == "Title")
            {
                Label label = obj as Label;
                title = label.Text;
            }     
            if (obj.AutomationId == "Artist")
            {
                Label label = obj as Label;
                artist = label.Text;
            }
        }

        foreach (Audio audio in Collection.ItemsSource)
        {
            if (audio.Title == title && audio.Artist == artist)
            {
                Collection.SelectedItem = audio;
                return;
            }
        }
    }
}