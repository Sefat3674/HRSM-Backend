using HRMS.DAL.Data;
using HRMS.DAL.Repositories;
using HRMS.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Add services to the container (clean JSON, no $id/$values)
builder.Services.AddControllers(); // removed ReferenceHandler.Preserve

// 🔹 Add DbContext
builder.Services.AddDbContext<HRMSDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// 🔹 Register Repositories (Dependency Injection)
builder.Services.AddScoped<IAttendanceRepo, AttendanceRepo>();

// 🔹 Add OpenAPI/Swagger
builder.Services.AddOpenApi();

// 🔹 Add CORS for Angular frontend
var frontendOrigin = "http://localhost:4200";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAngularApp",
        policy =>
        {
            policy.WithOrigins(frontendOrigin)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 🔹 Enable CORS
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();