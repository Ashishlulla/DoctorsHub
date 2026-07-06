using DoctorsHub.Application.Configuration;
using DoctorsHub.Application.Mapping;
using DoctorsHub.Domain.Identity;
using DoctorsHub.Infrastructure.Configurations;
using DoctorsHub.Web.Configurations;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



//Adding Required Services
//builder.Services.AddApplication();
builder.Services.AddIdentityService();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHttpClientServices(builder.Configuration);
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));


//Swagger Services
var app = builder.Build();


//Add Identity Scope
using (var scope = app.Services.CreateScope()) 
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var useManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();


    await IdentitySeeder.SeedRolesAndAdminAsync(roleManager, useManager);
}
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    { 
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

   

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

//app.MapRazorPages()
//   .WithStaticAssets();

app.Run();
