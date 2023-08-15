// This CreateBuilder(args) method configures logging providers.
// We can also configure it manually in this place.
using BattleshipGame.API.Services;
using BattleshipGame.Data.DbContexts;
using BattleshipGame.Logic.Logic;
using BattleshipGame.Logic.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MY SERVICES
builder.Services.AddDbContext<BattleshipGameDbContext>(dbContextOptions =>
dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:BattleshipGameDBConnectionString"]));

builder.Services.AddScoped<IPlayersRepository, PlayerRepository>();
builder.Services.AddSingleton<IMessageService, MessageService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var serviceProvider = new ServiceCollection()
                .AddScoped<IValidationService, ValidationService>()
                .BuildServiceProvider();

// Tworzenie obiektu GameSettings i przekazanie wstrzykniêtego IValidationService
var gameSettings = new GameSettings(10, 10, 9, serviceProvider.GetRequiredService<IValidationService>());

var app = builder.Build();

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
