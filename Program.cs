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


// Реєструємо сервіси
// builder.Services.AddSingleton<ITimestampService, SystemTimestampService>();
// приклад заміни сервісу SystemTimestampService на UnixTimestampService
builder.Services.AddSingleton<ITimestampService, UnixTimestampService>();
// builder.Services.AddTransient<ITimestampService, UnixTimestampService>();

builder.Services.AddSingleton<IKdfService, PbKdf1Service>();
builder.Services.AddSingleton<IStorageService, FileStorageService>();


// Налаштування сесій - тривалого сховища, що дозволяє зберігати дані між запитами
// https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Реєстрація контексту даних - аналогічна до сервісів
builder.Services.AddDbContext<DataContext>(   // Метод реєстрації - AddDbContext
    options =>                                // options - те, що передається до 
        options                               // конструктора DataContext(DbContextOptions options)
        .UseSqlServer(                        // UseSqlServer - конфігурація для 
            builder.Configuration             // MS SQL Server
            .GetConnectionString("LocalMs")   // builder.Configuration - доступ до
        )                                     // файлів конфігурації (appsettings.json)
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

app.UseSession();   // Включення сесій

app.UseAuthSession();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using var scope = app.Services.CreateScope();
await using var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
await dataContext.Database.MigrateAsync();

app.Run();
/* Д.З. Створити сервіс генерування випадкового ОТР (one time password) 
 * числа заданої довжини, наприклад, 6 цифр (кількість цифр передається параметром)
 * Інжектувати до HomeController, вивести у складі довільного представлення.
 * * Доповнити контентом сторінки щодо Razor, Ioc, Intro
 */
