using MasterDetails.Models;
using MasterDetails.Repositories;
using MasterDetails.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<PatientDbContext>(op => op.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
var app = builder.Build();
app.UseStaticFiles();
app.MapDefaultControllerRoute();
app.Run();
