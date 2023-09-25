using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VkNet.Model;

namespace VKMusicApp.Models
{
    public class PlayerAudios
    {
        public ObservableCollection<Audio> Audios { get; set; }
        public Audio PlayingAudio { get; set; }
        public string PathToAudio { get; set; }
        public int AudioIndex { get; set; }
    }
}
