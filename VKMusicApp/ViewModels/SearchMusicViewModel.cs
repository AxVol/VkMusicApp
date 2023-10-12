using System.Collections.ObjectModel;
using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.Implementation;
using VKMusicApp.Services.Interfaces;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class SearchMusicViewModel : MusicLibrary
    {
        private readonly IVkService vkService;

        public ICommand SearchCommand { get; set; }

        public SearchMusicViewModel(IVkService VkService, IAudioPlayerService service, IFileService file)
        {
            vkService = VkService;
            AudioPlayerService = service;
            fileService = file;

            UnFocus = new Command(UnFocused);
            SearchFocusCommand = new Command(SearchFocus);
            SearchCommand = new Command(Search);
            OpenMusicCommand = new Command(OpenMusic);
            ShowPopUpCommand = new Command(ShowPopUp);

            ViewAudio = new ObservableCollection<Audio>();
        }

        private async void Search(object obj)
        {
            string musicName = obj as string;

            ViewAudio.Clear();

            await vkService.GetAudioByString(musicName, ViewAudio);
        }
    }
}
