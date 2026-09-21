using ManualiAPI.Middleware;
using ManualiAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Os dados vivem dentro dos services, então todos precisam ser Singleton
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IUsuarioService, UsuarioService>();
builder.Services.AddSingleton<IArtesaoService, ArtesaoService>();
builder.Services.AddSingleton<IAdmService, AdmService>();
builder.Services.AddSingleton<IPedidoService, PedidoService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();   // primeiro, para envolver tudo

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// No Docker e no perfil "http" não existe porta HTTPS, e o redirect falharia.
// Só redireciona quando uma porta HTTPS estiver realmente configurada.
if (!string.IsNullOrEmpty(builder.Configuration["ASPNETCORE_HTTPS_PORT"]))
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.MapControllers();

app.Run();