using DalDocBackend.Data;
using DalDocBackend.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.SimpleNotificationService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Authentication
builder.Services
    .AddAuthentication()
    .AddBearerToken(IdentityConstants.BearerScheme);

// Add Authorization
builder.Services.AddAuthorizationBuilder();

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("MySQLConnection"),
    new MySqlServerVersion(new Version(8, 0, 23))));

// Configure Identity
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

// aws configration
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonSimpleNotificationService>();
builder.Services.AddSingleton<S3Service>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") // Allow requests from this origin
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); // Allows sending credentials if needed
        });
});

builder.Services.AddSignalR();

var app = builder.Build();

app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Ensure authentication middleware is added
app.UseAuthorization();
app.UseCors("AllowSpecificOrigin");

app.MapControllers();
app.MapHub<ChatHub>("/chat");

app.Run();
