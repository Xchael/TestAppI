using Microsoft.EntityFrameworkCore;
using TestAppI.Services;
using TestAppI.Services.Interfaces;
using TestData.Data;
using TestData.Repos;
using TestData.Repos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

var connStirng = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddDbContext<TestAppIDbContext>(options => options.UseSqlServer(connStirng));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(GenericRepo<>));
builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(GenericRepo<>));
builder.Services.AddScoped<ITestAppRepo, TestAppRepo>();
builder.Services.AddScoped<ITestAppService, TestAppService>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
