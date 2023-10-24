using Newtonsoft.Json;
using VKMusicApp.Models;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.ObjectModel;
using Plugin.LocalNotification;
#if ANDROID
using Android.OS;
#endif

namespace VKMusicApp.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly string rootPath = String.Empty;
        private readonly M3U8ToMP3.M3U8ToMP3 m3U8ToMP3;

        public event DownloadHandler AudioDownloaded;
        public event DeleteHandler AudioDeleted;

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
                File.Delete($"{PathToSave}/{audio.Artist}-{audio.Title}.mp3");

                AudioDeleted?.Invoke(audio);
            }
        }

        public async Task SaveMusic(Audio audio)
        {
            if (audio.Url != null)
            {
                string filePath = $"{PathToSave}/{audio.Artist}-{audio.Title}.mp3";
                List<byte[]> mp3 = await m3U8ToMP3.Convert(audio.Url.ToString());

                using (FileStream fileStream = File.Create(filePath))
                {
                    foreach (byte[] ts in mp3)
                    {
                        fileStream.Write(ts, 0, ts.Length);
                    }
                }

                NotificationRequest notification = new NotificationRequest
                {
                    NotificationId = 3,
                    Title = "Трек скачен",
                    Description = $"{audio.Artist} - {audio.Title}",
                    Silent = true,
                };

                AudioDownloaded?.Invoke(audio);
                await LocalNotificationCenter.Current.Show(notification);

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

        public ObservableCollection<Audio> GetMusics()
        {
            List<Audio> audios = new List<Audio>();
            string[] files = Directory.GetFiles($"{PathToSave}");

            foreach (string file in files)
            {
                string filename = file.Split('/')[^1];
                string title = filename.Split('-')[1].Replace(".mp3", null);
                string artist = filename.Split('-')[0];
                int duration = 0; //fix this
                DateTime createAt = new FileInfo(file).CreationTime;

                AudioAlbum album = new AudioAlbum();
                AudioCover thumb = new AudioCover();

                thumb.Photo600 = "player.png";
                album.Thumb = thumb;
                    
                Audio audio = new Audio()
                {
                    Title = title,
                    Artist = artist,
                    Album = album, 
                    Duration = duration,
                    TrackCode = file,
                    Date = createAt
                };

                audios.Add(audio);
            }

            return new ObservableCollection<Audio>(audios.OrderByDescending(a => a.Date));
        }

        public async Task SetPathToSave(string path)
        {
            VkPlayerConfig config = await GetConfig();

            config.PathFileSave = path;
            await File.WriteAllTextAsync($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
        }

        public async Task DeleteLoginAndPass()
        {
            VkPlayerConfig config = await GetConfig();

            config.Login = "";
            config.Password = "";

            await File.WriteAllTextAsync($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
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

            await File.WriteAllTextAsync($"{pathToJson}/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
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
