using AnimeNotificationsBot.Api;
using AnimeNotificationsBot.Api.Configs;
using AnimeNotificationsBot.Api.Services;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.DAL;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    DataContext.Configure(options, builder.Configuration.GetConnectionString("PostgreSql")!);
});

var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);
var botConfiguration = botConfigurationSection.Get<BotConfiguration>()!;

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


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
}


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapPost($"{botConfiguration.Route}/{botConfiguration.BotToken}", async (NewtonsoftJsonUpdate update, IBotService botService,
    CancellationToken cancellationToken) => {
    await botService.HandleUpdateAsync(update, cancellationToken);
    return Results.Ok();
});


app.Run();
