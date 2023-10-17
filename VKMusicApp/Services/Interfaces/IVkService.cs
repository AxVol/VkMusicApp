using System.Collections.ObjectModel;
using VKMusicApp.ViewModels;
using VkNet.Model;

namespace VKMusicApp.Services.Implementation
{
    public interface IVkService
    {
        public ObservableCollection<Audio> GetAudios();
        public ObservableCollection<AudioPlaylist> GetPlayLists();
        public Task<IEnumerable<Audio>> GetAudioByString(string music, ObservableCollection<Audio> audios);
        public ObservableCollection<Audio> GetAudioById(long id);
    }
}
