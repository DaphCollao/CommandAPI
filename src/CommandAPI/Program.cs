using CommandAPI.Data;
using CommandAPI.Data.Implement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
builder.Services.AddDbContext<CommandContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));

// Add Controllers Service
builder.Services.AddControllers();

builder.Services.AddScoped<ICommandAPIRepo, SqlCommandAPIRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Add UseRouting
app.UseRouting();

// Add UseEndpoints MapController
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
