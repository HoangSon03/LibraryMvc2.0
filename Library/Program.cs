using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Library.Data;
using Library.Models;
using System.ComponentModel;
using System.Configuration;
using Library.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryContext") ?? throw new InvalidOperationException("Connection string 'LibraryContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IGenericRepository<Item>), typeof(ItemRepository));
builder.Services.AddScoped(typeof(IGenericRepository<BorrowingDetail>), typeof(DetailRepository));
builder.Services.AddScoped(typeof(IGenericRepository<BorrowingHistory>), typeof(HistoryRepository));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
