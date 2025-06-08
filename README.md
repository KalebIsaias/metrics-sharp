# metrics-sharp

Este projeto expõe métricas de uma aplicação ASP.NET Core e visualiza em dashboards do Grafana, usando Prometheus para coleta.

## Serviços

* Aplicativo ASP.NET Core na porta 8080
* Prometheus na porta 9090
* Grafana na porta 3000

## Execução

1. Clone o repositório
2. Execute `docker-compose up --build`
3. Acesse:

   * Aplicativo: [http://localhost:8080](http://localhost:8080)
   * Prometheus: [http://localhost:9090](http://localhost:9090)
   * Grafana: [http://localhost:3000](http://localhost:3000) (login padrão: admin / admin)

## ASP.NET Core

* Endpoint de métricas: `/metrics`
* Métricas expostas:

  * Counter: `metrics_requisicoes_total` (por rota)
  * Gauge: `metrics_valor_aleatorio`
  * Tempo de resposta (via `AddAspNetCoreInstrumentation`)

Rotas:

* `/`
* `/ping`
* `/lento` (delay de 2s)

## prometheus.yml

```yaml
global:
  scrape_interval: 5s

scrape_configs:
  - job_name: 'metrics'
    static_configs:
      - targets: ['metrics:8080']
```


## Queries no Grafana

* Taxa de Requisições por Segundo:

  ```
  sum(rate(http_server_request_duration_seconds_count[1m]))
  ```
* Latência P95:

  ```
  histogram_quantile(0.95, sum by (le) (rate(http_server_request_duration_seconds_bucket[1m])))
  ```
* Requisições por rota:

  ```
  sum by (rota) (metrics_requisicoes_total)
  ```
* Gauge:

  ```
  metrics_valor_aleatorio
  ```

## Dashboard

Gráficos criados:

* Valor do Gauge Aleatório
* Taxa de Requisições por Segundo
* Requisições por Rota
* Latência P95 (ms)

Exemplo:


