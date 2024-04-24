using Microsoft.EntityFrameworkCore;
using Infrastructure.Models;
using SearchAPI.Middleware;
using Serilog.Events;
using Serilog;
using Microsoft.OpenApi.Models;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.


builder.Services.AddCors(options =>
{
    
  List<string> allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<List<string>>();
  options.AddPolicy("AllowSpecificOrigin", builder =>
        {
            builder.WithOrigins(allowedOrigins.ToArray()) // allowed origin
                 .AllowAnyHeader()
                   .AllowAnyMethod();
   });
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Products Detail search API", Version = "v1" });
    c.EnableAnnotations();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer.SaveSearchHistoryCommandHandler).Assembly));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer.SearchQueryHandler).Assembly));

//Add logging file 
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();



string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(baseDirectory, "App_Data"));
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
var app = builder.Build(); 

// Configure the HTTP request pipeline.

app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();

app.Run();
