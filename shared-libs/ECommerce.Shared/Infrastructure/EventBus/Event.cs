namespace ECommerce.Shared.Infrastructure.EventBus;
public record Event
{
    public Event()
    {
        Id = Guid.NewGuid();
        CreatedDate = DateTime.UtcNow;
    }

    public Guid Id { get; set; }

    public DateTime CreatedDate { get; set; }
}
