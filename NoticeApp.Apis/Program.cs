using Microsoft.EntityFrameworkCore;
using NoticeApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region CORS
// CORS 사용 등록
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", 
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

    //참고용
    options.AddPolicy("AllowSpecific",
        builder => builder
        .WithOrigins("https://localhost:44356")
        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
        .WithHeaders("accept", "content-type", "origin", "X-TotalRecordCount"));
});
#endregion

builder.Services.AddDbContext<NoticeAppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

builder.Services.AddTransient<INoticeRepository, NoticeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS 사용 허용
app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
