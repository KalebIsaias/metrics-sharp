using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

// Medidor customizado
var meter = new Meter("Metrics.Metrics", "1.0");
var contadorRequisicoes = meter.CreateCounter<int>("metrics.requisicoes_total", description: "Número total de requisições processadas.");
var gaugeValorAtual = meter.CreateObservableGauge("metrics.valor_aleatorio", () =>
{
    var valor = Random.Shared.Next(0, 100);
    return new Measurement<int>(valor, new KeyValuePair<string, object?>("contexto", "teste"));
}, description: "Gauge com valor aleatório para exemplo.");

builder.Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddRuntimeInstrumentation()
            .AddMeter("Metrics.Metrics")
            .AddPrometheusExporter();
    });

var app = builder.Build();

app.MapPrometheusScrapingEndpoint(); // Expondo em /metrics

app.MapGet("/", () =>
{
    contadorRequisicoes.Add(1, KeyValuePair.Create<string, object?>("rota", "/"));
    return Results.Ok("Olá! Métricas ativas.");
});

app.MapGet("/ping", () =>
{
    contadorRequisicoes.Add(1, KeyValuePair.Create<string, object?>("rota", "/ping"));
    return Results.Ok("pong");
});

app.MapGet("/lento", async () =>
{
    contadorRequisicoes.Add(1, KeyValuePair.Create<string, object?>("rota", "/lento"));
    await Task.Delay(2000);
    return Results.Ok("Resposta lenta");
});

app.Run();
