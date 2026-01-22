using Conduit.Api.Extensions;
using Conduit.Api.Middlewares;
using Conduit.API.Security;
using Conduit.Application.Abstractions.Auth;
using Conduit.Application.DependencyInjection;
using Conduit.Infrastructure.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddSwaggerDocumentation();

builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

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

app.UseMiddleware<GatewayUserHeadersMiddleware>();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
