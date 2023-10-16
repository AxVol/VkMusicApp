using Newtonsoft.Json;
using VKMusicApp.Models;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.ObjectModel;
using VKMusicApp.Services.M3U8ToMP3;
#if ANDROID
using Android.OS;
#endif

namespace VKMusicApp.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly string rootPath = String.Empty;
        private readonly M3U8ToMP3.M3U8ToMP3 m3U8ToMP3;

        public string PathToSave => GetConfig().Result.PathFileSave;

        public FileService(M3U8ToMP3.M3U8ToMP3 converter)
        {
            m3U8ToMP3 = converter;

#if ANDROID
            rootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.ToString();
#endif
        }

        public async Task DeleteMusic(Audio audio)
        {
            if (await MusicInStorage(audio))
            {
                System.IO.File.Delete($"{PathToSave}/{audio.Artist}-{audio.Title}.mp3");
            }
        }

        public async Task SaveMusic(Audio audio)
        {
            if (audio.Url != null)
            {
                byte[] mp3 = await m3U8ToMP3.Convert(audio.Url.ToString());

                await System.IO.File.WriteAllBytesAsync($"{PathToSave}/{audio.Artist}-{audio.Title}.mp3", mp3);
                return;
            }
            await Shell.Current.CurrentPage.DisplayAlert("Ошибка", "Трек не был найден", "Назад");
        }

        public async Task<bool> MusicInStorage(Audio audio)
        {
            string[] files = Directory.GetFiles($"{PathToSave}");

            foreach (string file in files)
            {
                string[] fileSplit = file.Split('/')[^1].Split('-');
                string artist = fileSplit[0];
                string title = fileSplit[1].Split('.')[0];

                if (artist == audio.Artist && title == audio.Title)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<ObservableCollection<Audio>> GetMusics()
        {
            ObservableCollection<Audio> audios = new ObservableCollection<Audio>();
            string[] files = Directory.GetFiles($"{PathToSave}");

            foreach (string file in files)
            {
                var music = TagLib.File.Create(file);

                AudioAlbum album = new AudioAlbum();
                AudioCover thumb = new AudioCover();

                thumb.Photo600 = "player.png";
                album.Thumb = thumb;

                Audio audio = new Audio()
                {
                    Title = music.Tag.Title,
                    Artist = music.Tag.FirstPerformer,
                    Album = album,
                    Duration = Convert.ToInt32(music.Properties.Duration.TotalSeconds),
                    Url = new Uri(file)
                };

                audios.Add(audio);
            }

            return audios;
        }

        public async Task SetPathToSave(string path)
        {
            VkPlayerConfig config = await GetConfig();

            config.PathFileSave = path;
            await System.IO.File.WriteAllTextAsync($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
        }

        public async Task DeleteLoginAndPass()
        {
            VkPlayerConfig config = await GetConfig();

            config.Login = "";
            config.Password = "";

            await System.IO.File.WriteAllTextAsync($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
        }

        public async Task SetConfig(string login, string password)
        {
            string pathToJson = $"{rootPath}/Android/media/VkPlayer";
            VkPlayerConfig config;

            try
            {
                config = await GetConfig();
            }
            catch
            {
                config = new VkPlayerConfig();
            }

            config.Login = login;
            config.Password = password;

            if (!Directory.Exists(pathToJson))
            {
                Directory.CreateDirectory(pathToJson);
            }

            if (!Directory.Exists(config.PathFileSave))
            {
                Directory.CreateDirectory(config.PathFileSave);
            }

            await System.IO.File.WriteAllTextAsync($"{pathToJson}/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
        }

        public async Task<VkPlayerConfig> GetConfig()
        {
            using StreamReader reader = new StreamReader($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json");

            string json = reader.ReadToEnd();
            VkPlayerConfig config = JsonConvert.DeserializeObject<VkPlayerConfig>(json);

            return config;
        }
    }
}
