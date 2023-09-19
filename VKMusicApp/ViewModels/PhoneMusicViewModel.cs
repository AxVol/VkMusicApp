using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VKMusicApp.Core;
using VkNet;

namespace VKMusicApp.ViewModels
{
    public class PhoneMusicViewModel : MusicLibrary
    {
        private string searchText;

        public PhoneMusicViewModel(VkApi VKApi)
        {
            
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
