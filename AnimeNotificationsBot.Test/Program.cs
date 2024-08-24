using AnimeNotificationsBot.DAL;
using AnimeNotificationsBot.DAL.Entities;
using AnimeNotificationsBot.Quartz.AutoMapper;
using AnimeNotificationsBot.Quartz.Services;
using AutoMapper;
using KodikDownloader;
using Microsoft.EntityFrameworkCore;
using ParserAnimeGO;
using ParserAnimeGO.Models;
using System.Configuration;

var requestParseraHandler = new RequestParserHandler();
var requestParserFactory = new RequestParserFactory();
var parserFromIDocument = new ParserFromIDocument();
var uriFactory = new AnimeGoUriFactory();

var parserAnime = new ParserAnimeGo(requestParseraHandler, requestParserFactory, parserFromIDocument, uriFactory);


var dbOptionsBuilder = new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<DataContext>
{
    
};


var connectionString = "Server=localhost;Port=5432;Database=anime_notification_bot;User Id=sa;Password=P@ssw0rd";

DataContext.Configure(dbOptionsBuilder, connectionString);


var context = new DataContext(dbOptionsBuilder.Options);
await context.Database.MigrateAsync();


var configuration = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>(); 
});


var mapper = configuration.CreateMapper();

var kodikClient = new KodikClient();

var animeService = new AnimeService(parserAnime, context, mapper, uriFactory, kodikClient);

await animeService.UpdateNotificationsAsync();