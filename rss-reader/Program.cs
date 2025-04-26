using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DotNetEnv;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using rss_reader.Data;
using rss_reader.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var keysFolder = Path.Combine(Directory.GetCurrentDirectory(), "DataProtection-Keys");
Directory.CreateDirectory(keysFolder);

var dpBuilder = builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysFolder));

var certPath = Path.Combine(Directory.GetCurrentDirectory(), "certs", "key-protection.pfx");
var certPassword = builder.Configuration["DataProtection:CertificatePassword"];
if (File.Exists(certPath) && !string.IsNullOrEmpty(certPassword))
{
    dpBuilder.ProtectKeysWithCertificate(
        new X509Certificate2(certPath, certPassword, X509KeyStorageFlags.MachineKeySet)
    );
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<DbService>();
builder.Services.AddScoped<RssService>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpMethodOverride(new HttpMethodOverrideOptions { FormFieldName = "_method" });

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();