using MailAPI.Data;
using MailAPI.Data.Migrations;
using MailAPI.Data.Models;
using MailAPI.Services;
using MailAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IEmailService,EmailService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IContactService, ContactHistoryService>();
builder.Services.AddDbContext<DataContext>(x =>
                    x.UseSqlServer(builder.Configuration.GetConnectionString("MainDbConnection")));

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
