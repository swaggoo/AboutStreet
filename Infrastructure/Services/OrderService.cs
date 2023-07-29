using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;

namespace Infrastructure.Services;
public class OrderService : IOrderService
{
    private readonly IGenericRepository<Order> _orderRepo;
    private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IBasketRepository _basketRepo;

    public OrderService(IGenericRepository<Order> orderRepo, 
        IGenericRepository<DeliveryMethod> deliveryMethodRepo, 
        IGenericRepository<Product> productRepo,
        IBasketRepository basketRepo)
    {
        _orderRepo = orderRepo;
        _deliveryMethodRepo = deliveryMethodRepo;
        _productRepo = productRepo;
        _basketRepo = basketRepo;
    }

    public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
    {
        // Get basket from the repo
        var basket = await _basketRepo.GetBasketAsync(basketId);

        // Get items from the product repo
        var items = new List<OrderItem>();
        foreach (var item in basket.Items)
        {
            var productItem = await _productRepo.GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        // Get delivery method from the repo
        var deliveryMethod = await _deliveryMethodRepo.GetByIdAsync(deliveryMethodId);

        // Calculate subtotal
        var subtotal = items.Sum(i => i.Price *  i.Quantity);

        // Create order
        var order = new Order(buyerEmail, shippingAddress, deliveryMethod, items, subtotal);

        // TODO: Save to database

        // Return the Order
        return order;
    }

    public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Order> GetOrderByIdAsync(int orderId, string buyerEmail)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        throw new NotImplementedException();
    }
}
