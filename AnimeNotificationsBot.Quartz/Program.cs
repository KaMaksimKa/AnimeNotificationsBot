using AnimeNotificationsBot.BLL.Configs;
using AnimeNotificationsBot.BLL.Interfaces;
using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.Quartz.AutoMapper;
using AnimeNotificationsBot.Quartz.Configs;
using AnimeNotificationsBot.Quartz.JobOptions;
using AnimeNotificationsBot.Quartz.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using ParserAnimeGO.Interface;
using Quartz;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DataContext>(options =>
{
    DataContext.Configure(options, builder.Configuration.GetConnectionString("PostgreSql")!);
});

builder.Services.AddScoped<IAnimeGoUriFactory, AnimeGoUriFactory>();

var requestParserFactorySection = builder.Configuration.GetSection(ParserConfig.Configuration);
builder.Services.Configure<ParserConfig>(requestParserFactorySection);
var parserConfig = requestParserFactorySection.Get<ParserConfig>()!;
builder.Services.AddScoped<IRequestParserFactory, RequestParserFactory>(services => new RequestParserFactory()
{
    Cookies = parserConfig.Cookies,
    UserAgent = parserConfig.UserAgent,
});

builder.Services.AddScoped<IRequestParserHandler, RequestParserHandler>(services => new RequestParserHandler()
{
    TimeBetweenRequest = TimeSpan.FromSeconds(parserConfig.TimeBetweenRequestFromSeconds),
});

builder.Services.AddScoped<IAnimeParserFromIDocument,ParserFromIDocument>();
builder.Services.AddScoped<ParserAnimeGo>();

var quartzSection = builder.Configuration.GetSection(QuartzConfig.Configuration);
builder.Services.Configure<QuartzConfig>(quartzSection);

builder.Services.Configure<AnimeNotificationBotConfig>(builder.Configuration.GetSection(AnimeNotificationBotConfig.Configuration));

builder.Services.AddScoped<AnimeNotificationsBot.Quartz.Services.AnimeService>();

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.ConfigureOptions<UpdateAnimeNotificationOptions>();
builder.Services.ConfigureOptions<UpdateAnimesOptions>();
builder.Services.ConfigureOptions<UpdateAnimeCommentsOptions>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
}


app.MapGet("/ping", () => Results.Ok("Ok"));

app.MapGet("/kodikManifestLink", async (AnimeService animeService) => await animeService.GetKodikManifestLinkAsync());

app.MapPut("/kodikManifestLink", async ([FromBody]Dictionary<string,int> telegramDocumentIdsByManifestLink,AnimeService animeService) => await animeService.PutKodikManifestLinkAsync(telegramDocumentIdsByManifestLink));

app.Run();
