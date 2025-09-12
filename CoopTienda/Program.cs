using CoopTienda.AccesoDatos.Data;
using CoopTienda.AccesoDatos.Repositorio;
using CoopTienda.AccesoDatos.Repositorio.IRepositorio;
using CoopTienda.Modelo;
using CoopTienda.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<UsuarioAplicacion, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//Repositorio
builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Reglas de contraseña
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Inventario}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
