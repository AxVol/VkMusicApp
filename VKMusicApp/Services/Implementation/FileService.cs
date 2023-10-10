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

        public string PathToSave => GetToken().Result;

        public async Task DeleteMusic(Audio audio)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteToken()
        {
            VkPlayerConfig config = await GetConfig();

            config.Token = "";

            await File.WriteAllTextAsync($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
        }

        public async Task<string> GetToken()
        {
            VkPlayerConfig config = await GetConfig();

            return config.Token;
        }

        public async Task SaveMusic(Audio audio)
        {
            throw new NotImplementedException();
        }

        public async Task SetPathToSave(string path)
        {
            throw new NotImplementedException();
        }

        public async Task SetToken(string token)
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

            config.Token = token;

            if (!Directory.Exists(pathToJson))
            {
                Directory.CreateDirectory(pathToJson);
            }

            await File.WriteAllTextAsync($"{pathToJson}/VkPlayerConfig.json", JsonConvert.SerializeObject(config));
        }

        private async Task<VkPlayerConfig> GetConfig()
        {
            using StreamReader reader = new StreamReader($"{rootPath}/Android/media/VkPlayer/VkPlayerConfig.json");

            string json = await reader.ReadToEndAsync();
            VkPlayerConfig config = JsonConvert.DeserializeObject<VkPlayerConfig>(json);

            return config;
        }
    }
}
