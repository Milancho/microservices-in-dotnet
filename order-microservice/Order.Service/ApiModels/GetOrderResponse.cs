namespace Order.Service.ApiModels;

public record GetOrderProductResponse(string ProductId, int Quantity);
public record GetOrderResponse(string CustomerId, Guid OrderId, DateTime OrderDate, List<GetOrderProductResponse> OrderProducts);
