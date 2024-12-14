using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(option => option.Filters.Add<AuthorizationFilter>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "請輸入 JWT Token，格式為 [token] 或 Bearer [token]\r\n\r\n如果需要測試用預設Token請使用\r\n\r\n" +
        "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZGVudGlmaWNhdGlvbiI6ImFzODYwMjE1In0.M0tRVQ6B8eKH5VY23AXalXQqaBi9yKMPape4pX9L-EW0V56x72O3T1DrQfsKjWhC0o8uIBumz9uQM4tb90dSiQ"
    });

    options.OperationFilter<HeaderTokenOperationFilter>();
});

var databaseConnection = builder.Configuration.GetSection("Database");
builder.Services.AddDbContext<BloggingContext>(options => options.UseSqlServer(databaseConnection.Value));
builder.Services.AddHttpClient();
builder.Services.AddExceptionHandler<MainExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
