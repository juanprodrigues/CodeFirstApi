using CodeFirstApi.Repositories;
using CodeFirstApi.Repositories.Interfaces;
using CodeFirstApi.Services;
using CodeFirstApi.Services.Interfaces;
using DB;
using Microsoft.EntityFrameworkCore;
using Scrutor;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BarContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("BarConnection")
    ));

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()

    .AddClasses()

    .AsImplementedInterfaces()

    .WithScopedLifetime()
);

//builder.Services
//    .AddControllers()
//    .AddJsonOptions(options =>
//    {
//        options.JsonSerializerOptions.ReferenceHandler =
//            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
//    });

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(classes => classes.Where(type =>
        type.Name.EndsWith("Service") ||
        type.Name.EndsWith("Repository")))
    .AsImplementedInterfaces()
    .WithScopedLifetime()
);
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";

builder.WebHost.UseUrls($"http://localhost:{port}");

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var context = scope.ServiceProvider.GetRequiredService<BarContext>();
//    context.Database.Migrate();
//}

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();