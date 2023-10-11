using CommunityToolkit.Maui.Storage;
using Newtonsoft.Json;
using VKMusicApp.Models;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;
using Microsoft.Maui.Controls.PlatformConfiguration;
#if ANDROID
using Android.OS;
#endif

namespace VKMusicApp.Services.Implementation
{
    public class FileService : IFileService
    {
        private readonly IFileSaver fileSaver;
        private readonly string rootPath = String.Empty;

        public FileService(IFileSaver FileSaver)
        {
            fileSaver = FileSaver;

#if ANDROID
            rootPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath.ToString();
#endif
        }

        public string PathToSave => GetConfig().Result.PathFileSave;

        public async Task DeleteMusic(Audio audio)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteLoginAndPass()
        {
            VkPlayerConfig config = await GetConfig();

            config.Login = "";
            config.Password = "";

            await File.WriteAllTextAsync($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
        }

        public async Task SaveMusic(Audio audio)
        {
            throw new NotImplementedException();
        }

        public async Task SetPathToSave(string path)
        {
            throw new NotImplementedException();
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
