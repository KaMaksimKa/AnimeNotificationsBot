using AnimeNotificationsBot.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


CreateHostBuilder(args).Build().Run();


static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            string dbSettings = hostContext.Configuration.GetConnectionString("PostgreSql")!;

            services.AddDbContext<DataContext>(options =>
            {
                DataContext.Configure(options, dbSettings);
            });
        });