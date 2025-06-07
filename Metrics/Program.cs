using OpenTelemetry.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics.AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    });

var app = builder.Build();

app.MapPrometheusScrapingEndpoint(); // Expondo o endpoint /metrics para o Prometheus

app.MapGet("/", () => "Aplicação ASP.NET Core 8.0 com métricas!");

app.Run();