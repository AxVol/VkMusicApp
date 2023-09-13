using Microsoft.Extensions.Logging;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VKMusicApp.Pages;
using VKMusicApp.ViewModels;

namespace VKMusicApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddAudioBypass();
		builder.Services.AddSingleton(new VkApi(builder.Services));
		//builder.Services.AddSingleton<IVkApiProvider>(new VkApiProvider(new VkApi(builder.Services)));

        builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<MusicLibraryPage>();

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<MusicLibraryViewModel>();

        return builder.Build();
	}
}
