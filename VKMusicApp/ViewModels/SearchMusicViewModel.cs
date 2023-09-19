using System.Collections.ObjectModel;
using System.Windows.Input;
using VKMusicApp.Core;
using VKMusicApp.Services.Implementation;
using VkNet.Model;

namespace VKMusicApp.ViewModels
{
    public class SearchMusicViewModel : MusicLibrary
    {
        private string searchText;
        private readonly IVkService vkService;

        public ICommand SearchCommand { get; set; }

        public SearchMusicViewModel(IVkService VkService)
        {
            vkService = VkService;

            UnFocus = new Command(UnFocused);
            SearchFocusCommand = new Command(SearchFocus);
            SearchCommand = new Command(Search);
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                
                if (!searchText.Any())
                    searchButtonStatus = false;

                searchButtonStatus = true;
                OnPropertyChanged();
            }
        }

        public bool SearchButtonStatus
        {
            get => searchButtonStatus;
            set
            {
                searchButtonStatus = value;
                OnPropertyChanged();
            }
        }

        private void Search(object obj)
        {
            string musicName = obj as string;

            ViewAudio = vkService.GetAudio(musicName);
        }
    }
}
