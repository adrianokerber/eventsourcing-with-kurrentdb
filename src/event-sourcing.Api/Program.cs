using event_sourcing.Domain.PayrollLoan;
using event_sourcing.Infrastructure.AppStartup;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEventStoreDb(builder.Configuration);
builder.Services.AddScoped<PayrollLoansRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(config =>
{
    config.Title = "Payroll Loan API";
    config.Version = "v1";
    config.Description = "API for managing payroll loans using event sourcing";
});

// Add MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();

// Configure Swagger/OpenAPI
app.UseOpenApi();
app.UseSwaggerUi();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}

app.Run();