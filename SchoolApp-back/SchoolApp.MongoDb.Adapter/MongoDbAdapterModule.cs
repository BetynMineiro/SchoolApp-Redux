using SchoolApp.CrossCutting;
using SchoolApp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SchoolApp.MongoDb.Adapter.Context;
using SchoolApp.MongoDb.Adapter.Repository;

namespace SchoolApp.MongoDb.Adapter;

public static class MongoDbAdapterModule
{
    public static void ConfigureMongoAdapterLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IMongoClient>(c =>
        {
            var appSettingsConfig = configuration.GetAppSettingsApiConfig();

            return MongoDbContext.BuildMongoConnection(appSettingsConfig.MongoConfig.Connection,
                appSettingsConfig.MongoConfig.Database);
        });

        services.AddScoped(c => c.GetService<IMongoClient>().StartSession());

        services.AddScoped<ISchoolRepository, SchoolRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

    }
}