using Angular;
using Angular.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddBffProxy(builder.Configuration);
builder.Services.AddLocalAuthentication();
builder.Services.AddHttpClient();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();
app.MapEndpoints(builder.Configuration);

// This is convenient for routing requests for dynamic content to a SPA framework, while also allowing requests for non-existent files to
// result in an HTTP 404.
app.MapFallbackToFile("index.html");

app.Run();