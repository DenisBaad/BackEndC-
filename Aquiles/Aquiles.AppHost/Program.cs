var builder = DistributedApplication.CreateBuilder(args);
var appHostDirectory = Path.GetFullPath("Infra");

// Prometheus para métricas
var prometheus = builder.AddContainer("prometheus", "prom/prometheus", "latest")
    .WithBindMount(Path.Combine(appHostDirectory, "prometheus.yml"), "/etc/prometheus/prometheus.yml")
    .WithEndpoint(name: "http-metrics", port: 9090, targetPort: 9090);

// Loki para logs
var loki = builder.AddContainer("loki", "grafana/loki", "2.9.0")
    .WithEndpoint(name: "http-logs", port: 3100, targetPort: 3100);

// Jaeger para traces
var jaeger = builder.AddContainer("jaeger", "jaegertracing/all-in-one", "1.53")
    .WithEndpoint(name: "http-traces", port: 16686, targetPort: 16686);

// Grafana para visualização de dashboards
var grafana = builder.AddContainer("grafana", "grafana/grafana", "10.1.0")
    .WithBindMount(Path.Combine(appHostDirectory, "grafana"), "/etc/grafana/provisioning")
    .WithEndpoint(name: "http-dashboard", port: 3000, targetPort: 3000)
    .WithReferenceRelationship(loki)
    .WithReferenceRelationship(jaeger);

// Kafka e Zookeeper
var zookeeper = builder.AddContainer("zookeeper", "confluentinc/cp-zookeeper", "7.4.1")
    .WithEnvironment("ZOOKEEPER_CLIENT_PORT", "2181")
    .WithEnvironment("ZOOKEEPER_TICK_TIME", "2000")
    .WithEndpoint(2181, targetPort: 2181);

var kafka = builder.AddContainer("kafka", "confluentinc/cp-kafka", "7.4.1")
    .WithEnvironment("KAFKA_BROKER_ID", "1")
    .WithEnvironment("KAFKA_ZOOKEEPER_CONNECT", "zookeeper:2181")
    .WithEnvironment("KAFKA_ADVERTISED_LISTENERS", "PLAINTEXT://localhost:9092")
    .WithEnvironment("KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR", "1")
    .WithEnvironment("KAFKA_AUTO_CREATE_TOPICS_ENABLE", "true")
    .WithEndpoint(9092, targetPort: 9092)
    .WaitFor(zookeeper);

// Microservices (com referência para observability)
var aquilesApi = builder.AddProject<Projects.Aquiles_API>("aquiles-api")
    .WithReferenceRelationship(prometheus)
    .WithReferenceRelationship(loki)
    .WithReferenceRelationship(jaeger);

var enderecosApi = builder.AddProject<Projects.Enderecos_API>("enderecos-api")
    .WithReferenceRelationship(prometheus)
    .WithReferenceRelationship(loki)
    .WithReferenceRelationship(jaeger);

// Conecta Prometheus aos microservices
prometheus.WithReferenceRelationship(aquilesApi)
          .WithReferenceRelationship(enderecosApi);

builder.Build().Run();