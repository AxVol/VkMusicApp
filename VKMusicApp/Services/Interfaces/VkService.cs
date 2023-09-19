using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKMusicApp.Services.Implementation;
using VKMusicApp.ViewModels;
using VkNet;
using VkNet.Model;

namespace VKMusicApp.Services.Interfaces
{
    public class VkService : IVkService
    {
        private VkApi vkApi;

        public VkService(VkApi VKApi)
        {
            vkApi = VKApi;
        }

        public ObservableCollection<Audio> GetAudios(AccountMusicViewModel vm)
        {
            var music = vkApi.Audio.Get(new AudioGetParams()
            {
                OwnerId = vkApi.UserId
            }).ToList();

            return new ObservableCollection<Audio>(music);
        }

        public ObservableCollection<Audio> GetAudios(PhoneMusicViewModel vm)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Audio> GetPlayList()
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<Audio> GetAudio(string music)
        {
            var audios = vkApi.Audio.Search(new AudioSearchParams()
            {
                Query = music,
                Autocomplete = true,
                Sort = VkNet.Enums.AudioSort.Popularity

            }).ToList();

            return new ObservableCollection<Audio>(audios);
        }
    }
}
