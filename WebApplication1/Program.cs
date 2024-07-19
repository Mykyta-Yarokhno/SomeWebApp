using WebApplication1;
using WebApplication1.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Identity.Data;
using System.Configuration;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("WebCoffeeDbContextConnection") ?? throw new InvalidOperationException("Connection string 'WebCoffeeDbContextConnection' not found.");

builder.Services.AddDbContext<WebCoffeeDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<WebCoffeeDbContext>();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<CoffeeService>();
builder.Services.AddScoped<LogResponceContent>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();;


app.UseAuthorization();

app.UseStaticFiles();

app.MapRazorPages();


app.MapControllers();

app.Run();
