var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Add Controllers Service
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//Add UseRouting
app.UseRouting();

// app.MapGet("/", async context => {
//     await context.Response.WriteAsync("Hello World");
// });
//Add UseEndpoint MapController
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();