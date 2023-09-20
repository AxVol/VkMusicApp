using System.Collections.ObjectModel;
using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Services.Implementation;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class SearchMusicViewModel : MusicLibrary
    {
        private readonly IVkService vkService;

        public ICommand SearchCommand { get; set; }

        public SearchMusicViewModel(IVkService VkService)
        {
            vkService = VkService;

            UnFocus = new Command(UnFocused);
            SearchFocusCommand = new Command(SearchFocus);
            SearchCommand = new Command(Search);

            ViewAudio = new ObservableCollection<Audio>();
        }

        private async void Search(object obj)
        {
            string musicName = obj as string;

            ViewAudio.Clear();

            await vkService.GetAudio(musicName, ViewAudio);
        }
    }
}
