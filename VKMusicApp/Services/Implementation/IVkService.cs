using System.Collections.ObjectModel;
using VKMusicApp.ViewModels;
using VkNet.Model;

namespace VKMusicApp.Services.Implementation
{
    public interface IVkService
    {
        public ObservableCollection<Audio> GetAudios(AccountMusicViewModel vm);
        public ObservableCollection<Audio> GetAudios(PhoneMusicViewModel vm);
        public ObservableCollection<Audio> GetPlayList();
        public Audio GetAudio();
    }
}
