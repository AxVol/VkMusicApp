using VKMusicApp.DataProviders.Interfaces;
using VkNet;

namespace VKMusicApp.DataProviders.Implementations
{
    public class VkApiProvider : IVkApiProvider
    {
        public VkApi VkApi { get; set; }

        public VkApiProvider(VkApi vkApi) => VkApi = vkApi;
    }
}
