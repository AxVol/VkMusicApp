using VKMusicApp.Models;
using VkNet.Model;

namespace VKMusicApp.Services.Interfaces
{
    public interface IFileService
    {
        public string PathToSave { get; }

        public Task SaveMusic(Audio audio);
        public Task DeleteMusic(Audio audio);
        public Task<VkPlayerConfig> GetConfig();
        public Task SetConfig(string login, string password);
        public Task DeleteLoginAndPass();
        public Task SetPathToSave(string path);
    }
}
