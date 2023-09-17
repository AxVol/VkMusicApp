using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VKMusicApp.Pages;
using VKMusicApp.ViewModels;
using CommunityToolkit.Maui;

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

        builder.Services.AddTransient<LoginPage>();
		builder.Services.AddSingleton<MusicLibraryPage>();
		builder.Services.AddTransient<PlaylistPage>();
		builder.Services.AddSingleton<PhoneMusicPage>();
		builder.Services.AddTransient<SearchMusicPage>();

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddSingleton<MusicLibraryViewModel>();
		builder.Services.AddTransient<PlaylistViewModel>();
		builder.Services.AddSingleton<PhoneMusicViewModel>();
		builder.Services.AddTransient<SearchMusicViewModel>();

        return builder.Build();
	}
}
