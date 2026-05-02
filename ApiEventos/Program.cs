using ApiEventos.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// STRING DE CONEXÃO VINDO DO appsettings.json
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// SERVICES
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// GARANTE QUE O BANCO SERÁ CRIADO AUTOMATICAMENTE
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // ESSA LINHA RESOLVE 90% DOS ERROS
}

// PIPELINE
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();