using AspNetcoreWebApp.Services;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);


builder.Host.ConfigureAppConfiguration(builder =>
{
    builder.AddAzureAppConfiguration(options =>
    {
        options.Connect("Endpoint=https://appconfigurationmanager.azconfig.io;Id=+mQG;Secret=eY0J4NYFHlrb/h7fGO8wYSHeI3u301FxR4t4CiCG2bc=").UseFeatureFlags();
    });
});

// Add services to the container.
builder.Services.AddRazorPages();


builder.Services.AddFeatureManagement();

builder.Services.AddTransient<IProductsService, ProductsService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();


app.Run();
