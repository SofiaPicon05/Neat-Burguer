using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Neat_Burguer.Models.Entities;
using Neat_Burguer.Repositories;

var builder = WebApplication.CreateBuilder(args);

string? Db = builder.Configuration.GetConnectionString("DbConnection");

builder.Services.AddDbContext<NeatContext>(x => x.UseMySql("server=localhost;user=root;password=root;database=neat",
 Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));


builder.Services.AddMvc();

builder.Services.AddTransient<MenuRepository>();
builder.Services.AddTransient<ClasificacionRepository>();



var app = builder.Build();

app.UseFileServer();

app.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.Run();
