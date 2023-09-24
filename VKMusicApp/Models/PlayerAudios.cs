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

        public string UrlConverter(Uri Url)
        {
            string url = Regex.Replace(
                Url.ToString(),
                @"/[a-zA-Z\d]{6,}(/.*?[a-zA-Z\d]+?)/index.m3u8()",
                @"$1$2.mp3");

            return url;
        }
    }
}
