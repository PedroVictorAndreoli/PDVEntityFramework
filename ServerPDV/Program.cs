using Microsoft.EntityFrameworkCore;
using ServerPDV;
using ServerPDV.Interfaces;
using ServerPDV.Models;
using ServerPDV.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configurar Entity Framework Core para usar SQL Server (LocalDB)
builder.Services.AddDbContext<CaixaSupermercadoContext>(options =>
    options.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=CaixaSupermercado;Integrated Security=True;"));

builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ICrud<Item>, ItemRepository>();
builder.Services.AddScoped<ItemRepository>();
builder.Services.AddScoped<ICrud<Usuario>, UsuarioRepository>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<ICrud<Cupom>, CupomRepository>();
builder.Services.AddScoped<CupomRepository>();
builder.Services.AddScoped<ICrud<CupomItem>, CupomItemRepository>();
builder.Services.AddScoped<CupomItemRepository>();

var app = builder.Build();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minha API V1");
    c.RoutePrefix = ""; // Exibe o Swagger na raiz
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
