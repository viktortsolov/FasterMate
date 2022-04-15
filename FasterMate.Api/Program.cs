using FasterMate.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiDbContexts(builder.Configuration);
builder.Services.AddApiServices();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
