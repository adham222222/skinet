using CORE.Interfaces;
using Infrastructure.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<StoreContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));

var app = builder.Build();



app.MapControllers();

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


app.Run();
