using ASP_SPR311.Data;
using ASP_SPR311.Middleware;
using ASP_SPR311.Models;
using ASP_SPR311.Services.Kdf;
using ASP_SPR311.Services.Storage;
using ASP_SPR311.Services.Timestamp;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Ðåºñòðóºìî ñåðâ³ñè
// builder.Services.AddSingleton<ITimestampService, SystemTimestampService>();
// ïðèêëàä çàì³íè ñåðâ³ñó SystemTimestampService íà UnixTimestampService
builder.Services.AddSingleton<ITimestampService, UnixTimestampService>();
// builder.Services.AddTransient<ITimestampService, UnixTimestampService>();

builder.Services.AddSingleton<IKdfService, PbKdf1Service>();
builder.Services.AddSingleton<IStorageService, FileStorageService>();


// Íàëàøòóâàííÿ ñåñ³é - òðèâàëîãî ñõîâèùà, ùî äîçâîëÿº çáåð³ãàòè äàí³ ì³æ çàïèòàìè
// https://learn.microsoft.com/ru-ru/aspnet/core/fundamentals/app-state?view=aspnetcore-9.0
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Ðåºñòðàö³ÿ êîíòåêñòó äàíèõ - àíàëîã³÷íà äî ñåðâ³ñ³â
builder.Services.AddDbContext<DataContext>(   // Ìåòîä ðåºñòðàö³¿ - AddDbContext
    options =>                                // options - òå, ùî ïåðåäàºòüñÿ äî 
        options                               // êîíñòðóêòîðà DataContext(DbContextOptions options)
        .UseSqlServer(                        // UseSqlServer - êîíô³ãóðàö³ÿ äëÿ 
            builder.Configuration             // MS SQL Server
            .GetConnectionString("LocalMs")   // builder.Configuration - äîñòóï äî
        )                                     // ôàéë³â êîíô³ãóðàö³¿ (appsettings.json)
);
builder.Services.AddScoped<DataAccessor>();


builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new DoubleBinderProvider());
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{    
    options.AddPolicy(
        name:"CorsPolicy",
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyHeader();
        });
});


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

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.UseSession();   // Âêëþ÷åííÿ ñåñ³é

app.UseAuthSession();

app.UseAuthToken();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


using var scope = app.Services.CreateScope();
await using var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
await dataContext.Database.MigrateAsync();

app.Run();
/* Ä.Ç. Ñòâîðèòè ñåðâ³ñ ãåíåðóâàííÿ âèïàäêîâîãî ÎÒÐ (one time password) 
 * ÷èñëà çàäàíî¿ äîâæèíè, íàïðèêëàä, 6 öèôð (ê³ëüê³ñòü öèôð ïåðåäàºòüñÿ ïàðàìåòðîì)
 * ²íæåêòóâàòè äî HomeController, âèâåñòè ó ñêëàä³ äîâ³ëüíîãî ïðåäñòàâëåííÿ.
 * * Äîïîâíèòè êîíòåíòîì ñòîð³íêè ùîäî Razor, Ioc, Intro
 */
// Edited from Github

// Edited from Rider
