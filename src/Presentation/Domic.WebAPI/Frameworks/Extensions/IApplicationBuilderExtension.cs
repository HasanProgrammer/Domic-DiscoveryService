using Domic.Core.Common.ClassExtensions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace Domic.WebAPI.Frameworks.Extensions;

public static class IApplicationBuilderExtension
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="builder"></param>
    public static void RegisterMongoClient(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DBContext>(provider =>
            new DBContext("ServiceRegistry",
                MongoClientSettings.FromConnectionString( 
                    provider.GetRequiredService<IConfiguration>().GetMongoConnectionString() 
                )
            )
        );
    }
}