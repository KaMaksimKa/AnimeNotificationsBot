using AnimeNotificationsBot.Api.Configs;
using AnimeNotificationsBot.Api.Services;
using AnimeNotificationsBot.Api.Services.Interfaces;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);





var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);
var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>(httpClient =>
    {
        TelegramBotClientOptions options = new(botConfiguration!.BotToken);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddScoped<IBotService, BotService>();
builder.Services.AddScoped<IBotSender, BotSender>();

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
