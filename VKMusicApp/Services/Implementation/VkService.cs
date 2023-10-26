using System.Collections.ObjectModel;
using VKMusicApp.Services.Implementation;
using VkNet;
using VkNet.Model;
using VkNet.Utils;

namespace VKMusicApp.Services.Interfaces
{
    public class VkService : IVkService
    {
        private readonly VkApi vkApi;

        public VkService(VkApi VKApi)
        {
            vkApi = VKApi;
        }

        public ObservableCollection<Audio> GetAudios()
        {
            VkCollection<Audio> music = vkApi.Audio.Get(new AudioGetParams()
            {
                OwnerId = vkApi.UserId
            });

            ParallelLoopResult parallelLoopResult = Parallel.ForEach<Audio>(music, SetThumbAsync);

            return new ObservableCollection<Audio>(music);
        }

        public ObservableCollection<AudioPlaylist> GetPlayLists()
        {
            VkCollection<AudioPlaylist> playlists = vkApi.Audio.GetPlaylists((long)vkApi.UserId);

            ParallelLoopResult parallelLoopResult = Parallel.ForEach<AudioPlaylist>(playlists, 
                (playlist) => 
                {
                    if (playlist.Photo == null)
                    {
                        AudioCover photo = new AudioCover();

                        photo.Photo600 = "playlist.png";

                        playlist.Photo = photo;
                    }
                    else if (playlist.Photo.Photo600 == string.Empty)
                    {
                        playlist.Photo.Photo600 = "playlist.png";
                    }
                });

            return new ObservableCollection<AudioPlaylist>(playlists);
        }

        public ObservableCollection<Audio> GetAudioById(long id)
        {
            VkCollection<Audio> music = vkApi.Audio.Get(new AudioGetParams()
            {
                OwnerId = vkApi.UserId,
                PlaylistId = id
            });

            ParallelLoopResult parallelLoopResult = Parallel.ForEach<Audio>(music, SetThumbAsync);

            return new ObservableCollection<Audio>(music);
        }

        public async Task<IEnumerable<Audio>> GetAudioByString(string music, ObservableCollection<Audio> audios)
        {
            VkCollection<Audio> musics = await vkApi.Audio.SearchAsync(new AudioSearchParams()
            {
                Query = music,
                Autocomplete = true,
                Sort = VkNet.Enums.AudioSort.Popularity

            });

            audios = new ObservableCollection<Audio>(musics);
            ParallelLoopResult parallelLoopResult = Parallel.ForEach<Audio>(audios, SetThumbAsync);

            return audios;
        }

        // Устанавливает базовую картинку для треков у которых их нету в базах ВК
        private void SetThumbAsync(Audio audio)
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
    }
}
