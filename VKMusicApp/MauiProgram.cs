using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VKMusicApp.Pages;
using VKMusicApp.ViewModels;
using VKMusicApp.Services.Implementation;
using VKMusicApp.Services.Interfaces;
using VKMusicApp.Services.AudioPlayer.Interfaces;
using VKMusicApp.Services.AudioPlayer.Implementation;
using CommunityToolkit.Maui.Storage;

namespace VKMusicApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
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

		builder.Services.AddSingleton<IFileSaver>(FileSaver.Default);
		builder.Services.AddTransient<IVkService, VkService>();
		builder.Services.AddSingleton<IAudioPlayerService, AudioPlayerService>();
		builder.Services.AddSingleton<IFileService, FileService>();

        builder.Services.AddTransient<LoginPage>();
		builder.Services.AddSingleton<AccountMusicPage>();
		builder.Services.AddSingleton<PlaylistPage>();
		builder.Services.AddSingleton<PhoneMusicPage>();
		builder.Services.AddTransient<SearchMusicPage>();
		builder.Services.AddTransient<MusicPlaylistPage>();
		builder.Services.AddSingleton<AudioPlayerPage>();
		builder.Services.AddTransient<SettingsPage>();

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddSingleton<AccountMusicViewModel>();
		builder.Services.AddSingleton<PlaylistViewModel>();
		builder.Services.AddSingleton<PhoneMusicViewModel>();
		builder.Services.AddTransient<SearchMusicViewModel>();
		builder.Services.AddTransient<MusicPlaylistViewModel>();
		builder.Services.AddSingleton<AudioPlayerViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

        return builder.Build();
	}
}
