using System.Collections.ObjectModel;
using VKMusicApp.Services.Implementation;
using VKMusicApp.ViewModels;
using VkNet;
using VkNet.Model;
using VkNet.Utils;

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
            });

            music = SetThumb(music);

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

        public async Task<IEnumerable<Audio>> GetAudio(string music, ObservableCollection<Audio> audios)
        {
            var musics = await vkApi.Audio.SearchAsync(new AudioSearchParams()
            {
                Query = music,
                Autocomplete = true,
                Sort = VkNet.Enums.AudioSort.Popularity

            });

            musics = SetThumb(musics);

            foreach (var audio in musics)
            {
                audios.Add(audio);
            }

            return audios;
        }

        private VkCollection<Audio> SetThumb(VkCollection<Audio> audios)
        {
            foreach (Audio audio in audios)
            {
                if (audio.Album == null)
                {
                    AudioAlbum album = new AudioAlbum();
                    AudioCover thumb = new AudioCover();

                    thumb.Photo600 = "player.png";
                    album.Thumb = thumb;

                    audio.Album = album;
                }
                else if (audio.Album.Thumb.Photo600 == String.Empty)
                {
                    audio.Album.Thumb.Photo600 = "player.png";
                }
            }

            return audios;
        }
    }
}
