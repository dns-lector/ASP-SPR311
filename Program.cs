using ASP_SPR311.Data;
using ASP_SPR311.Middleware;
using ASP_SPR311.Services.Kdf;
using ASP_SPR311.Services.Storage;
using ASP_SPR311.Services.Timestamp;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// �������� ������
// builder.Services.AddSingleton<ITimestampService, SystemTimestampService>();
// ������� ����� ������ SystemTimestampService �� UnixTimestampService
builder.Services.AddSingleton<ITimestampService, UnixTimestampService>();
// builder.Services.AddTransient<ITimestampService, UnixTimestampService>();

builder.Services.AddSingleton<IKdfService, PbKdf1Service>();
builder.Services.AddSingleton<IStorageService, FileStorageService>();


// ������������ ���� - ��������� �������, �� �������� �������� ��� �� ��������
// https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ��������� ��������� ����� - ��������� �� ������
builder.Services.AddDbContext<DataContext>(   // ����� ��������� - AddDbContext
    options =>                                // options - ��, �� ���������� �� 
        options                               // ������������ DataContext(DbContextOptions options)
        .UseSqlServer(                        // UseSqlServer - ������������ ��� 
            builder.Configuration             // MS SQL Server
            .GetConnectionString("LocalMs")   // builder.Configuration - ������ ��
        )                                     // ����� ������������ (appsettings.json)
);                                            // 


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

app.UseSession();   // ��������� ����

app.UseAuthSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using var scope = app.Services.CreateScope();
await using var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
await dataContext.Database.MigrateAsync();

app.Run();
/* �.�. �������� ����� ����������� ����������� ��� (one time password) 
 * ����� ������ �������, ���������, 6 ���� (������� ���� ���������� ����������)
 * ����������� �� HomeController, ������� � ����� ��������� �������������.
 * * ��������� ��������� ������� ���� Razor, Ioc, Intro
 */
