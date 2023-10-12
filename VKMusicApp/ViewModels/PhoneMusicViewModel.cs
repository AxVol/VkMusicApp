using VKMusicApp.Core;
using VKMusicApp.Services.Interfaces;
using VkNet;

namespace VKMusicApp.ViewModels
{
    public class PhoneMusicViewModel : MusicLibrary
    {
        private string searchText;

        public PhoneMusicViewModel(IFileService file)
        {
            fileService = file;

            ShowPopUpCommand = new Command(ShowPopUp);
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                searchText = value;
                
                OnPropertyChanged();
            }
        }
    }
}
