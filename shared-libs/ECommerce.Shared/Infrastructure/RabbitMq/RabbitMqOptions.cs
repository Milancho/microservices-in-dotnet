namespace ECommerce.Shared.Infrastructure.RabbitMq;

public class RabbitMqOptions
{
    public const string RabbitMqSectionName = "RabbitMq";

    public string HostName { get; set; } = string.Empty;
}
