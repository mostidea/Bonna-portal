using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ?? Swagger - JWT Bearer Token ayarý
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Bonna API", Version = "v1" });

  var securityScheme = new OpenApiSecurityScheme
  {
    Name = "Authorization",
    Description = "Bearer token girin. Örnek: Bearer {token}",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.Http,
    Scheme = "bearer",
    BearerFormat = "JWT"
  };

  c.AddSecurityDefinition("Bearer", securityScheme);

  var securityRequirement = new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    };

  c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bonna API V1");
    c.RoutePrefix = "swagger";
  });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
