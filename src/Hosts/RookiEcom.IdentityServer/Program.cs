var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

var appSettings = new AppSettings();
configuration.Bind(appSettings);

builder.Services.AddInfrastructure(configuration);
builder.Services.AddDuendeIdentityConfiguration(configuration);
builder.Services.AddAuthentication(configuration);
builder.Services.AddIdentityServiceCollection();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClients", policy =>
        policy.WithOrigins("https://localhost:5001", "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();

builder.Services
    .AddSwaggerDocumentation()
    .AddVersioning();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // app.UseExceptionHandler("/Home/Error");
        
    app.UseHsts();
}
else
{
    // app.UseSwagger();
// app.UseSwaggerUI();
    app.UseSwaggerDocumentation();   
}
app.UseExceptionHandler();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowClients");

app.UseRouting();
app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();