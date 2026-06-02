using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Prog_part_2.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Prog_part_2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Prog_part_2Context") ?? throw new InvalidOperationException("Connection string 'Prog_part_2Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient("ClientsApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5084/");
});

builder.Services.AddHttpClient("ContractsApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5084/");
});
builder.Services.AddHttpClient("ServiceRequestsApi", client =>
{
    client.BaseAddress = new Uri("http://localhost:5084/");
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();


