using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using SengokuProvider.Library.Services.Common;
using SengokuProvider.Library.Services.Events;
using SengokuProvider.Library.Services.Legends;
using SengokuProvider.Library.Services.Players;
using SengokuProvider.Library.Services.Users;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

//constant config variables
var connectionString = builder.Configuration["ConnectionStrings:AlexandriaConnectionString"];
var graphQLUrl = builder.Configuration["GraphQLSettings:Endpoint"];
var bearerToken = builder.Configuration["GraphQLSettings:Bearer"];

// Add services to the container.
builder.Services.AddTransient<CommandProcessor>();
builder.Services.AddSingleton<IntakeValidator>();
builder.Services.AddScoped(provider => new GraphQLHttpClient(graphQLUrl, new NewtonsoftJsonSerializer())
{
    HttpClient = { DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", bearerToken) } }
});

builder.Services.AddScoped<ICommonDatabaseService, CommonDatabaseService>(provider =>
{
    return new CommonDatabaseService(connectionString);
});
builder.Services.AddScoped<IUserService, UserService>(provider =>
{
    var intakeValidator = provider.GetRequiredService<IntakeValidator>();
    return new UserService(connectionString, intakeValidator);
});
builder.Services.AddScoped<IEventIntakeService, EventIntakeService>(provider =>
{
    var intakeValidator = provider.GetService<IntakeValidator>();
    var graphQlClient = provider.GetService<GraphQLHttpClient>();
    var queryService = provider.GetService<IEventQueryService>();
    return new EventIntakeService(connectionString, graphQlClient, queryService, intakeValidator);
});
builder.Services.AddScoped<ILegendQueryService, LegendQueryService>(provider =>
{
    var graphQlClient = provider.GetService<GraphQLHttpClient>();
    return new LegendQueryService(connectionString, graphQlClient);
});
builder.Services.AddScoped<IPlayerQueryService, PlayerQueryService>(provider =>
{
    var graphQlClient = provider.GetService<GraphQLHttpClient>();
    return new PlayerQueryService(connectionString, graphQlClient);
});
builder.Services.AddScoped<IPlayerIntakeService, PlayerIntakeService>(provider =>
{
    var playerQueryService = provider.GetService<IPlayerQueryService>();
    var legendQueryService = provider.GetService<ILegendQueryService>();
    return new PlayerIntakeService(connectionString, playerQueryService, legendQueryService);
});
builder.Services.AddScoped<IEventQueryService, EventQueryService>(provider =>
{
    var intakeValidator = provider.GetService<IntakeValidator>();
    var graphQlClient = provider.GetService<GraphQLHttpClient>();
    return new EventQueryService(connectionString, graphQlClient, intakeValidator);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
