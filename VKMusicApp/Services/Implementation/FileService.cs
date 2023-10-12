using Newtonsoft.Json;
using VKMusicApp.Models;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
#if ANDROID
using Android.OS;
#endif

namespace VKMusicApp.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly string rootPath = String.Empty;

        public string PathToSave => GetConfig().Result.PathFileSave;

        public FileService()
        {
#if ANDROID
            rootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.ToString();
#endif
        }

        public async Task DeleteMusic(Audio audio)
        {
            if (await MusicInStorage(audio))
            {
                File.Delete($"{PathToSave}/{audio.Artist}-{audio.Title}.mp3");
            }
        }

        public async Task SaveMusic(Audio audio)
        {
            string url = Regex.Replace(
                audio.Url.ToString(),
                @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
                @"$1$2.mp3");

            using (HttpClient client = new HttpClient())
            {
                using Stream readStream = await client.GetStreamAsync(url);
                using Stream writeStream = File.Open($"{PathToSave}/{audio.Artist}-{audio.Title}.mp3", FileMode.CreateNew);

                await readStream.CopyToAsync(writeStream);
            }
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
            throw new NotImplementedException();
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
