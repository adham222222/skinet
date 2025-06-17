using API.Middleware;
using CORE.Entities;
using CORE.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddCors();

builder.Services.AddAuthorization();

builder.Services.AddIdentityApiEndpoints<AppUser>().
AddEntityFrameworkStores<StoreContext>();

var app = builder.Build();


app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x=>x.AllowAnyHeader().AllowAnyMethod().AllowCredentials(). 
        WithOrigins("http://localhost:4200","https://localhost:4200"));

app.MapControllers();

app.MapGroup("api").MapIdentityApi<AppUser>();

app.UseStaticFiles();
app.UseDefaultFiles();

try
{
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    var service = services.GetRequiredService<StoreContext>();

    await service.Database.MigrateAsync();

    await StoreContextSeed.SeedAsycn(service);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    throw;
}

app.MapGet("/", () => "SkiNet API is running!");

app.MapGet("/api/status", () => new
{
    Status = "Online",
    Timestamp = DateTime.UtcNow,
    Runtime = "dotnet-9.0-linux-x64"
});

app.Run();
