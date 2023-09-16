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
		builder.Services.AddTransient<MusicLibraryPage>();
		builder.Services.AddTransient<PlaylistPage>();
		builder.Services.AddTransient<PhoneMusicPage>();
		builder.Services.AddTransient<SearchMusicPage>();

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<MusicLibraryViewModel>();
		builder.Services.AddTransient<PlaylistViewModel>();
		builder.Services.AddTransient<PhoneMusicViewModel>();
		builder.Services.AddTransient<SearchMusicViewModel>();

        return builder.Build();
	}
}
