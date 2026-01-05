using Conduit.Api.Authentication;
using Conduit.Api.Middleware;
using Conduit.Application.DependencyInjection;
using Conduit.Infrastructure.Persistence.DependencyInjection;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine($"AuthService:BaseUrl = {builder.Configuration["AuthService:BaseUrl"]}");

var authServiceUrl = builder.Configuration["AuthService:BaseUrl"];

if (string.IsNullOrWhiteSpace(authServiceUrl))
    throw new InvalidOperationException("AuthService:BaseUrl não configurado.");

builder.Services.AddHttpContextAccessor();

builder.Services.AddHttpClient<AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri(authServiceUrl);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

builder
    .Services.AddAuthentication("Bff")
    .AddScheme<AuthenticationSchemeOptions, BffAuthenticationHandler>("Bff", _ => { });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "Frontend",
        policy => policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200")
    );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Frontend");

app.UseRouting();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<EnsureProfileMiddleware>();

app.MapControllers();

app.Run();
