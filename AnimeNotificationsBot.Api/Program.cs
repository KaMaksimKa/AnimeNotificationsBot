using AnimeNotificationsBot.Api;
using AnimeNotificationsBot.Api.Configs;
using AnimeNotificationsBot.Api.Quartz.JobOptions;
using AnimeNotificationsBot.Api.Services;
using AnimeNotificationsBot.Api.Services.Interfaces;
using AnimeNotificationsBot.BLL.AutoMapper;
using AnimeNotificationsBot.BLL.Configs;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.BLL.Services;
using AnimeNotificationsBot.DAL;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Quartz;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(options =>
{
    DataContext.Configure(options, builder.Configuration.GetConnectionString("PostgreSql")!);
});

var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);
builder.Services.Configure<QuartzConfig>(builder.Configuration.GetSection(QuartzConfig.Configuration));
builder.Services.Configure<AnimeConfig>(builder.Configuration.GetSection(AnimeConfig.Configuration));


var botConfiguration = botConfigurationSection.Get<BotConfiguration>()!;
builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>(httpClient =>
    {
        TelegramBotClientOptions options = new(botConfiguration!.BotToken);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services.AddScoped<IBotSender, BotSender>();
builder.Services.AddScoped<ICommandFactory, ReflectionCommandFactory>();
builder.Services.AddScoped<IBotService, BotService>();
builder.Services.AddScoped<IAnimeService, AnimeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBotMessageGroupService, BotMessageGroupService>();
builder.Services.AddScoped<ICallbackQueryDataService, CallbackQueryDataService>();
builder.Services.AddScoped<IAnimeSubscriptionService, AnimeSubscriptionService>();
builder.Services.AddScoped<IDubbingService, DubbingService>();
builder.Services.AddScoped<IAnimeNotificationService, AnimeNotificationService>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();


builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService();
builder.Services.ConfigureOptions<NotifyAboutAnimeOptions>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));

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

app.UseStaticFiles();

app.MapPost($"{botConfiguration.Route}/{botConfiguration.BotToken}", async (NewtonsoftJsonUpdate update, IBotService botService,
    CancellationToken cancellationToken) => {
    await botService.HandleUpdateAsync(update, cancellationToken);
    return Results.Ok();
});

app.MapGet("/ping", () => Results.Ok("Ok"));

app.Run();
