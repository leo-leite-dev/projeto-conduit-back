using System.Net.Http.Headers;
using Conduit.Api.Authentication;
using Conduit.Api.Extensions;
using Conduit.Api.Middlewares;
using Conduit.Application.DependencyInjection;
using Conduit.Infrastructure.Persistence.DependencyInjection;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

builder.Services.AddHttpClient<AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["AuthService:BaseUrl"]
            ?? throw new InvalidOperationException("AuthService:BaseUrl não configurado")
    );

    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json")
    );
});

builder
    .Services.AddAuthentication("Bff")
    .AddScheme<AuthenticationSchemeOptions, BffAuthenticationHandler>("Bff", _ => { });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = null;
});

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

app.MapControllers();

app.Run();
