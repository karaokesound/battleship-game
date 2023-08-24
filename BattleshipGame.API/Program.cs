// This CreateBuilder(args) method configures logging providers.
// We can also configure it manually in this place.
using BattleshipGame.API.Services;
using BattleshipGame.API.Services.Controllers;
using BattleshipGame.API.Services.Repositories;
using BattleshipGame.Data.DbContexts;
using BattleshipGame.Logic.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BattleshipGame.API", Version = "v1" });
});

// MY SERVICES

builder.Services.AddDbContext<BattleshipGameDbContext>(dbContextOptions =>
dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:BattleshipGameDBConnectionString"]));

builder.Services.AddScoped<IPlayersRepository, PlayerRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IValidationService, ValidationService>();
builder.Services.AddSingleton<IGeneratingService, GeneratingService>();
builder.Services.AddScoped<IFieldRepository, FieldRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameStartingService, GameStartingService>();
builder.Services.AddScoped<IGameInfoService, GameInfoService>();
builder.Services.AddScoped<IGamePlayService, GamePlayService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<BattleshipGameDbContext>();

    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
