using ASP_SPR311.Services.Timestamp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Реєструємо сервіси
// builder.Services.AddSingleton<ITimestampService, SystemTimestampService>();
// приклад заміни сервісу SystemTimestampService на UnixTimestampService
builder.Services.AddSingleton<ITimestampService, UnixTimestampService>();
// builder.Services.AddTransient<ITimestampService, UnixTimestampService>();




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
/* Д.З. Створити сервіс генерування випадкового ОТР (one time password) 
 * числа заданої довжини, наприклад, 6 цифр (кількість цифр передається параметром)
 * Інжектувати до HomeController, вивести у складі довільного представлення.
 * * Доповнити контентом сторінки щодо Razor, Ioc, Intro
 */
