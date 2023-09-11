using VKMusicApp.Services.Interfaces;
using VkNet;

namespace VKMusicApp.Services.Implementations
{
    public class VkApiProvider : IVkApiProvider
    {
        public VkApi VkApi { get; set; }

        public VkApiProvider(VkApi vkApi) => VkApi = vkApi;
    }
}
