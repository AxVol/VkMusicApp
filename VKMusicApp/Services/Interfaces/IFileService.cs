using System.Collections.ObjectModel;
using VKMusicApp.Models;
using VkNet.Model;

namespace VKMusicApp.Services.Interfaces
{
    public delegate void DownloadHandler(Audio audio);
    public delegate void DeleteHandler(Audio audio);

    public interface IFileService
    {
        public event DownloadHandler AudioDownloaded;
        public event DeleteHandler AudioDeleted;

        public string PathToSave { get; }

        public Task SaveMusic(Audio audio);
        public Task DeleteMusic(Audio audio);
        public ObservableCollection<Audio> GetMusics();
        public Task<VkPlayerConfig> GetConfig();
        public Task SetConfig(string login, string password);
        public Task DeleteLoginAndPass();
        public Task SetPathToSave(string path);
        public Task<bool> MusicInStorage(Audio audio);
    }
}
