using LocalChatBotBackend.Interfaces;
using LocalChatBotBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddSingleton<IProjectAnalyzerService, ProjectAnalyzerService>();


var app = builder.Build();

// Enable CORS
app.UseCors();

// Map controllers
app.MapControllers();

app.Run();
