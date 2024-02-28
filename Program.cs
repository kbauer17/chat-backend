using Chat.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// add the CORS service
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "Hubs",
        builder =>
        {
            builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                // Anonymous origins NOT allowed for web sockets
                .WithOrigins("https://localhost:7196","https://kbauer17.github.io")
                .AllowCredentials();
        });
});

// add the service for SignalR
builder.Services.AddSignalR();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// add routing and designate the Hub with Cors
app.UseRouting();
app.UseCors("Hubs");


app.UseAuthorization();

// replace: app.MapControllers(); with the following mappings:
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/hubs/chat");
});

app.Run();
