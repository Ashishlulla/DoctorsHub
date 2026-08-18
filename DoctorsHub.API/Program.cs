using DoctorsHub.API.Configurations;
using DoctorsHub.API.Middleware;
using DoctorsHub.Application.Configuration;
using DoctorsHub.Infrastructure.Configurations; 
using System.Text.Json.Serialization;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

//Adding services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddIdentityService(builder.Configuration);
builder.Services.AddSwaggerServices();


//Quest Pdf
QuestPDF.Settings.License = LicenseType.Community;
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
