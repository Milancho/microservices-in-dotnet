namespace ECommerce.Shared.Observability;

public class OpenTelemetryOptions
{
    public const string OpenTelemetrySectionName = "OpenTelemetry";

    public string OtlpExporterEndpoint { get; set; } = string.Empty;
}
