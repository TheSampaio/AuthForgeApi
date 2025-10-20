var builder = WebApplication.CreateBuilder(args);

// Add usage of controllers
builder.Services.AddControllers();

// Add Swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Uses Swagger UI during development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Maps all controller classes
app.MapControllers();

app.Run();
