using Microsoft.EntityFrameworkCore;
using SampleApp.Exceptions;
using SampleApp.Models;
using SampleApp.Repository;
using FluentValidation.AspNetCore;
using FluentValidation;
using SampleApp.Validators;
using SampleApp.Services;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("EmployeeDB");
        builder.Services.AddDbContext<EmployeeContext>(x => x.UseSqlServer(connectionString));
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ControllerExceptionFilter>();
        });
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddFluentValidationClientsideAdapters();
        builder.Services.AddValidatorsFromAssemblyContaining<EmployeeModelValidator>();
        builder.Services.AddAutoMapper(typeof(Program).Assembly);
        builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
        builder.Services.AddTransient<IEmployeeService, EmployeeService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/error");
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}