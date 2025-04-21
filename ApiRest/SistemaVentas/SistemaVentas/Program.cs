using Microsoft.EntityFrameworkCore;
using SistemaVentas.Data;
using SistemaVentas.Mappings;
using SistemaVentas.Services;
using SistemaVentas.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Agrega el servicio CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Origen de tu frontend
                  .AllowAnyMethod() // Permite todos los métodos (GET, POST, etc.)
                  .AllowAnyHeader(); // Permite cualquier header
        });
});

// Add services to the container.
builder.Services.AddDbContext<PruebaTecnicaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IVentaService, VentaService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usa la política CORS
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();