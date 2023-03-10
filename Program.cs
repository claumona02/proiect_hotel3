using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using proiect_hotel2.Data;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
   policy.RequireRole("Admin"));
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Reservations");
    options.Conventions.AllowAnonymousToPage("/Reservations/Index");
    options.Conventions.AllowAnonymousToPage("/Reservations/Details");
    options.Conventions.AuthorizeFolder("/Guests", "AdminPolicy");
    options.Conventions.AuthorizeFolder("/Categories", "AdminPolicy");
});
builder.Services.AddDbContext<proiect_hotel2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("proiect_hotel2Context") ?? throw new InvalidOperationException("Connection string 'proiect_hotel2Context' not found.")));

builder.Services.AddDbContext<LibraryIdentityContext>(options =>

options.UseSqlServer(builder.Configuration.GetConnectionString("proiect_hotel2Context") ?? throw new InvalidOperationException("Connectionstring 'proiect_hotel2Context' not found.")));
builder.Services.AddDefaultIdentity<IdentityUser>(options =>options.SignIn.RequireConfirmedAccount = false)
   .AddRoles<IdentityRole>()
 .AddEntityFrameworkStores<LibraryIdentityContext>();

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
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.Run();
