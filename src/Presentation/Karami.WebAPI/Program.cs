using Karami.Core.Grpc.Service;
using Karami.Core.Infrastructure.Extensions;
using Karami.Core.WebAPI.Extensions;
using Karami.WebAPI.EntryPoints.GRPCs;
using Karami.WebAPI.Frameworks.Extensions;
using Karami.WebAPI.Frameworks.Jobs;

/*-------------------------------------------------------------------*/

WebApplicationBuilder builder = WebApplication.CreateBuilder();

#region Configs

builder.WebHost.ConfigureAppConfiguration((context, builder) => builder.AddJsonFiles(context.HostingEnvironment));

#endregion

/*-------------------------------------------------------------------*/

#region Service Container

builder.RegisterHelpers();
builder.RegisterCommandQueryUseCases();
builder.RegisterQueryRepositories();
builder.RegisterJobs();
builder.RegisterMessageBroker();
builder.RegisterEventsSubscriber();
builder.RegisterGrpcServer();

builder.Services.AddMvc();
builder.Services.AddHostedService<HealthCheckJob>();

#endregion

/*-------------------------------------------------------------------*/

WebApplication application = builder.Build();

/*-------------------------------------------------------------------*/

#region Middleware

if (application.Environment.IsProduction())
{
    application.UseHsts();
    application.UseHttpsRedirection();
}

application.UseRouting();

application.UseEndpoints(endpoints => {

    #region GRPC's Services

    endpoints.MapGrpcService<ServiceRPC>();

    #endregion

});

#endregion

/*-------------------------------------------------------------------*/

//HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

application.Run();

/*-------------------------------------------------------------------*/

//For Integration Test

public partial class Program {}