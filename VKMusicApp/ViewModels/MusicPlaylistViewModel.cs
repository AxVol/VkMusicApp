using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    [QueryProperty(nameof(ViewAudio), nameof(ViewAudio))]
    public class MusicPlaylistViewModel : MusicLibrary
    {
        public MusicPlaylistViewModel()
        {
            ViewAudio = new ObservableCollection<Audio>();
        }
    }
}
