using Microsoft.EntityFrameworkCore;
using Product_API.Context;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/*DbContext DepenendencyInjection */

var dbHost = "localhost";
var dbName = "microservices";
var dbPassword = "mahi14321";
var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};Trusted_Connection=true;TrustServerCertificate=true;";

builder.Services.AddDbContext<StoreContext>(opt =>opt.UseSqlServer(connectionString));
//ADD-JSON-SERIALIZER
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
//CORS implementation
builder.Services.AddCors(options =>
{
    options.AddPolicy("products", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("products");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
