

using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.Quartz.Configs;
using AnimeNotificationsBot.Quartz.JobOptions;
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

var requestParserFactorySection = builder.Configuration.GetSection(RequestParserFactoryConfig.Configuration);
builder.Services.Configure<RequestParserFactoryConfig>(requestParserFactorySection);
var requestParserFactoryConfig = requestParserFactorySection.Get<RequestParserFactoryConfig>()!;
builder.Services.AddScoped<IRequestParserFactory, RequestParserFactory>(factory => new RequestParserFactory()
{
    Cookies = requestParserFactoryConfig.Cookies,
    UserAgent = requestParserFactoryConfig.UserAgent,
});

builder.Services.AddScoped<IRequestParserHandler, RequestParserHandler>();
builder.Services.AddScoped<IAnimeParserFromIDocument,ParserFromIDocument>();
builder.Services.AddScoped<ParserAnimeGo>();

var quartzSection = builder.Configuration.GetSection(QuartzConfig.Configuration);
builder.Services.Configure<QuartzConfig>(quartzSection);

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService();

builder.Services.ConfigureOptions<AnimeNotificationOptions>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();
}


app.Run();
