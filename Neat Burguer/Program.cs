using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Neat_Burguer.Models.Entities;
using Neat_Burguer.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<NeatContext>(x => x.UseMySql("server=localhost;user=root;password=root;database=neat",
 Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql")));


builder.Services.AddTransient<MenuRepository>();
builder.Services.AddTransient<ClasificacionRepository>();

builder.Services.AddMvc();
var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapDefaultControllerRoute();

app.Run();
