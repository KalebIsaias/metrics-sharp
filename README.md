# metrics-sharp

Este projeto exp�e m�tricas de uma aplica��o ASP.NET Core e visualiza em dashboards do Grafana, usando Prometheus para coleta.

## Servi�os

* Aplicativo ASP.NET Core na porta 8080
* Prometheus na porta 9090
* Grafana na porta 3000

## Execu��o

1. Clone o reposit�rio
2. Execute `docker-compose up --build`
3. Acesse:

   * Aplicativo: [http://localhost:8080](http://localhost:8080)
   * Prometheus: [http://localhost:9090](http://localhost:9090)
   * Grafana: [http://localhost:3000](http://localhost:3000) (login padr�o: admin / admin)

## ASP.NET Core

* Endpoint de m�tricas: `/metrics`
* M�tricas expostas:

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

* Taxa de Requisi��es por Segundo:

  ```
  sum(rate(http_server_request_duration_seconds_count[1m]))
  ```
* Lat�ncia P95:

  ```
  histogram_quantile(0.95, sum by (le) (rate(http_server_request_duration_seconds_bucket[1m])))
  ```
* Requisi��es por rota:

  ```
  sum by (rota) (metrics_requisicoes_total)
  ```
* Gauge:

  ```
  metrics_valor_aleatorio
  ```

## Dashboard

Gr�ficos criados:

* Valor do Gauge Aleat�rio
* Taxa de Requisi��es por Segundo
* Requisi��es por Rota
* Lat�ncia P95 (ms)

Exemplo:


