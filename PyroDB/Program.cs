using Mica;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PyroDB.Data;
using PyroDB.Application.Jobs.PyroData;
using PyroDB.Application.Jobs.PyroData.Synchronizers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options
    .UseLazyLoadingProxies()
    .UseMySQL(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddScoped<PyroDataRecipeSynchronizer>();
builder.Services.AddScoped<PyroDataChemicalSynchronizer>();

builder.Services.AddMicaScheduler(scheduler => scheduler
    .AddJob<SyncPyroDataRecipeJob>(job => job
        .AddTrigger(trg => trg
            .Interval = TimeSpan.FromDays(30)))
    //.AddJob<SyncPyroDataChemicalJob>(job => job
    //    .AddTrigger(trg => trg
    //        .Interval = TimeSpan.FromDays(30)))
    );




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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
