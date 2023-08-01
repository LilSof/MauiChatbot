using System.Reflection;
using Microsoft.AspNetCore.Components.WebView.Maui;
using ChatBotErazmus.Data;
using ChatBotErazmus.Services;
using Microsoft.Extensions.Configuration;

namespace ChatBotErazmus;

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
			});

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddSingleton<ChatGService>(cp =>
        {
            var config = cp.GetRequiredService<IConfiguration>();
            var apiUrl = config.GetValue<string>("ChatGPTSettings:ApiURL");
            var apiKey = config.GetValue<string>("ChatGPTSettings:ApiKey");
            return new ChatGService(apiUrl, apiKey);
        });

        var a = Assembly.GetExecutingAssembly();
        using var stream = a.GetManifestResourceStream("ChatBotErazmus.appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonStream(stream)
            .Build();


        builder.Configuration.AddConfiguration(config);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		
		builder.Services.AddSingleton<WeatherForecastService>();

		return builder.Build();
	}
}
