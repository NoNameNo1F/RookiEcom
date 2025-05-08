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

// Attach Modules Configurations
builder.Services.AddProductModule(
    opt => configuration.GetSection("ConnectionString").Bind(opt));

builder.Services.AddCartModule(
    opt => configuration.GetSection("ConnectionString").Bind(opt));

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services
    .AddSwaggerDocumentation()
    .AddVersioning();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
        policy.WithOrigins("https://localhost:5001", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});
builder.Services.AddAuthentication(configuration);
builder.Services.AddAuthorizationExtension();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumentation();
}

app.UseCors("AllowClients");
app.UseHttpsRedirection();
app.UseExceptionHandler(_ => { });

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();