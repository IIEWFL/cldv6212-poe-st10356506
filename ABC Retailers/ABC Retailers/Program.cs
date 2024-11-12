using ABC_Retailers.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ABC_Retailers.Models;
using System.Globalization;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);

//adding services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ABCRetailersDBContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("abcSql")));


var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
