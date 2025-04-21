var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.WebHost.UseLogger(configuration =>
{
    var appSettings = new AppSettings();
    configuration.Bind(appSettings);
    return appSettings.Logging;
});

var appSettings = new AppSettings();
configuration.Bind(appSettings);

builder.Services.AddInfrastructure(configuration);
builder.Services.AddAuthentication(configuration);
builder.Services.AddAuthorizationExtension();

// Attach Modules Configurations
builder.Services.AddProductModule(
    opt => configuration.GetSection("ConnectionString").Bind(opt));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
        policy.WithOrigins("https://localhost:5001", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services
    .AddSwaggerDocumentation()
    .AddVersioning();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseCors("AllowClients");
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();