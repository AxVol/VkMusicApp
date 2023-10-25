using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using VKMusicApp.Core;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Implementation;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public partial class SearchMusicViewModel : MusicLibrary
    {
        private readonly IVkService vkService;

        public SearchMusicViewModel(IVkService VkService, IAudioPlayerService service, IFileService file)
        {
            vkService = VkService;
            AudioPlayerService = service;
            FileService = file;

            ViewAudio = new ObservableCollection<Audio>();
        }

        [RelayCommand]
        private async Task Search(object obj)
        {
            string musicName = obj as string;

            ViewAudio.Clear();

            await vkService.GetAudioByString(musicName, ViewAudio);
        }
    }
}
