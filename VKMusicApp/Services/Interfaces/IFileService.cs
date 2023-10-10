using VkNet.Model;

namespace VKMusicApp.Services.Interfaces
{
    public interface IFileService
    {
        public string PathToSave { get; }

        public Task SaveMusic(Audio audio);
        public Task DeleteMusic(Audio audio);
        public Task<string> GetToken();
        public Task SetToken(string token);
        public Task DeleteToken();
        public Task SetPathToSave(string path);
    }
}
