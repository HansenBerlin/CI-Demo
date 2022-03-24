using Microsoft.Net.Http.Headers;
using PaymentCore.Repositories;
using PaymentInfrastructure.Common;
using PaymentInfrastructure.Repositorys;
using PaymentInfrastructure.ServicesController;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISqlService, SqlService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISavingsAccountRepository, SavingAccountsRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors(policy => 
    policy.WithOrigins("https://localhost:5000", "http://localhost:5001")
        .AllowAnyMethod()
        .WithHeaders(HeaderNames.ContentType));
app.UseAuthorization();
app.MapControllers();
app.Run();