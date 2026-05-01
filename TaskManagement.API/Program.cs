
using TaskManagement.API.Configuration;
using TaskManagement.Application.AuthHelper;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.IServices;
using TaskManagement.Application.Services;
using TaskManagement.Infrastructure.DataSeed;
using TaskManagement.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// ================= SERVICES =================

// Controllers
builder.Services.AddControllers();

// Identity + DB
builder.Services.ConfigureIdentity(builder.Configuration);

// JWT
builder.Services.ConfigureJwtToken(builder.Configuration);

// Swagger
builder.Services.ConfigureSwagger(builder.Configuration);

// Custom services (DI)
builder.Services.ConfigureExtinction(builder.Configuration);

// Redis Cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Background Queue + Worker
builder.Services.AddSingleton<ITaskQueue, TaskQueue>();
builder.Services.AddHostedService<TaskBackgroundService>();
// Custom services
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAdminService, AdminService>();


// HttpContext (Auth)
builder.Services.AddHttpContextAccessor();

// Static config (temporary workaround)
JwtTokenExtensions.config = builder.Configuration;

// ================= BUILD APP =================
var app = builder.Build();

// ================= MIDDLEWARE =================

// Global Exception Handling
app.UseMiddleware<ExceptionMiddleware>();

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS
app.UseHttpsRedirection();

// CORS
app.UseCors("AllowAllOrigins");

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Static files
app.UseStaticFiles();

// Controllers
app.MapControllers();

// ================= SEED DB =================
app.InitializeDb();

// ================= RUN =================
app.Run();
