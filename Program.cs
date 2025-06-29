using Film_Ai.Data.DbContext;
using Film_Ai.Data.Services.Implementation;
using Film_Ai.Data.Services.Interface;
using Film_Ai.Jobs;
using Film_Ai.Models.Common;
using Film_Ai.Settings;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Quartz;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<TMDbSettings>(builder.Configuration.GetSection("TMDb"));
// Add services to the container.
builder.Services.AddScoped<ITMDbService, TMDbService>();
builder.Services.AddHttpClient<ITMDbService, TMDbService>();

// Configure MongoDB
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
    return new MongoClient(settings?.ConnectionString);
});

builder.Services.AddScoped(sp =>
{
    var mongoClient = sp.GetRequiredService<IMongoClient>();
    var settings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
    return new MongoDbContext(mongoClient, settings.DatabaseName);
});


builder.Services.AddScoped<IMovieService, MovieService>();
//quartz Job    
builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("CalendarDataSynchronizerJob");
    q.AddJob<FetchMoviesJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("CalendarDataSynchronizerJob-trigger")
        .WithCronSchedule("0 0 0 1 * ?")); // First day of every month at 03:00 AM});
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.AddControllers();

// Configure Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.DocumentTitle = "Persian Calendar API";
});
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
