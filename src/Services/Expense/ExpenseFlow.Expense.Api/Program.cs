using ExpenseFlow.Expense.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog Logging
builder.Host.UseSerilogLogging();

// Add services to the container.
builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddApiVersioningConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddAuthServices(builder.Configuration);
builder.Services.AddHealthCheckConfig(builder.Configuration);
builder.Services.AddProblemDetailsConfig();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerConfig();
}

app.UseExceptionHandler(); // Maps native IExceptionHandler (GlobalExceptionHandler)
app.UseStatusCodePages(); // ProblemDetails integration

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecksConfig();

app.Run();
