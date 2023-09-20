using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    [QueryProperty(nameof(ViewAudio), nameof(ViewAudio))]
    public class MusicPlaylistViewModel : ObservableObject
    {
        private ObservableCollection<Audio> viewAudio;

        public bool ViewAudioIsVisable { get; set; } = true;
        public ObservableCollection<Audio> ViewAudio 
        {
            get => viewAudio;
            set 
            {
                viewAudio = value;
                OnPropertyChanged();
            }
        }

        public MusicPlaylistViewModel()
        {
            ViewAudio = new ObservableCollection<Audio>();
        }
    }
}
