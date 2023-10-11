using VKMusicApp.Pages;

namespace VKMusicApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AccountMusicPage), typeof(AccountMusicPage));
            Routing.RegisterRoute(nameof(PlaylistPage), typeof(PlaylistPage));
            Routing.RegisterRoute(nameof(PhoneMusicPage), typeof(PhoneMusicPage));
            Routing.RegisterRoute(nameof(SearchMusicPage), typeof(SearchMusicPage));
            Routing.RegisterRoute(nameof(MusicPlaylistPage), typeof(MusicPlaylistPage));
            Routing.RegisterRoute(nameof(AudioPlayerPage), typeof(AudioPlayerPage));
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        }
    }
}