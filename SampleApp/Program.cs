using Microsoft.EntityFrameworkCore;
using SampleApp.Models;
using SampleApp.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("EmployeeDB");
builder.Services.AddDbContext<EmployeeContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
