using CommandAPI.Data;
using CommandAPI.Data.Implement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext
var sqlBuilder = new NpgsqlConnectionStringBuilder();
sqlBuilder.ConnectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
sqlBuilder.Username = builder.Configuration["UserID"];
sqlBuilder.Password = builder.Configuration["Password"];

builder.Services.AddDbContext<CommandContext>(opt =>
    opt.UseNpgsql(sqlBuilder.ConnectionString));

// Add Controllers Service
builder.Services.AddControllers().AddNewtonsoftJson(s => 
    {
        s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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
