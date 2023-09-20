using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VKMusicApp.Pages;
using VKMusicApp.ViewModels;
using CommunityToolkit.Maui;
using VKMusicApp.Services.Implementation;
using VKMusicApp.Services.Interfaces;

namespace VKMusicApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddAudioBypass();
		builder.Services.AddSingleton<VkApi>(new VkApi(builder.Services));

		builder.Services.AddTransient<IVkService, VkService>();

        builder.Services.AddTransient<LoginPage>();
		builder.Services.AddSingleton<AccountMusicPage>();
		builder.Services.AddSingleton<PlaylistPage>();
		builder.Services.AddSingleton<PhoneMusicPage>();
		builder.Services.AddTransient<SearchMusicPage>();
		builder.Services.AddTransient<MusicPlaylistPage>();

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddSingleton<AccountMusicViewModel>();
		builder.Services.AddSingleton<PlaylistViewModel>();
		builder.Services.AddSingleton<PhoneMusicViewModel>();
		builder.Services.AddTransient<SearchMusicViewModel>();
		builder.Services.AddTransient<MusicPlaylistViewModel>();

        return builder.Build();
	}
}
