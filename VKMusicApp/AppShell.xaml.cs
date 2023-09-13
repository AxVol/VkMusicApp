using VKMusicApp.Pages;

namespace VKMusicApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(MusicLibraryPage), typeof(MusicLibraryPage));
        }
    }
}