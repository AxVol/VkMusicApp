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
using VKMusicApp.Services.M3U8ToMP3;
using Plugin.LocalNotification;
using CommunityToolkit.Mvvm.Messaging;
using VKMusicApp.Services;

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
			.UseLocalNotification()
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

		builder.Services.AddSingleton<IMessenger, WeakReferenceMessenger>();
		builder.Services.AddTransient<IVkService, VkService>();
		builder.Services.AddSingleton<IAudioPlayerService, AudioPlayerService>();
		builder.Services.AddSingleton<IFileService, FileService>();
		builder.Services.AddTransient<M3U8ToMP3>();
		builder.Services.AddSingleton<PlaylistService>();

        builder.Services.AddTransient<LoginPage>();
		builder.Services.AddSingleton<AccountMusicPage>();
		builder.Services.AddSingleton<PlaylistPage>();
		builder.Services.AddSingleton<PhoneMusicPage>();
		builder.Services.AddSingleton<SearchMusicPage>();
		builder.Services.AddTransient<MusicPlaylistPage>();
		builder.Services.AddSingleton<AudioPlayerPage>();
		builder.Services.AddTransient<SettingsPage>();

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddSingleton<AccountMusicViewModel>();
		builder.Services.AddSingleton<PlaylistViewModel>();
		builder.Services.AddSingleton<PhoneMusicViewModel>();
		builder.Services.AddSingleton<SearchMusicViewModel>();
		builder.Services.AddTransient<MusicPlaylistViewModel>();
		builder.Services.AddSingleton<AudioPlayerViewModel>();
		builder.Services.AddTransient<SettingsViewModel>();

        return builder.Build();
	}
}
