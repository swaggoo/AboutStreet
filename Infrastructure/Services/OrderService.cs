using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services;
public class OrderService : IOrderService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IBasketRepository _basketRepo;

    public OrderService(IUnitOfWork unitOfWork,
        IBasketRepository basketRepo)
    {
        _unitOfWork = unitOfWork;
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
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
            var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
            var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
            items.Add(orderItem);
        }

        // Get delivery method from the repo
        var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

        // Calculate subtotal
        var subtotal = items.Sum(i => i.Price *  i.Quantity);

        // Create order
        var order = new Order(buyerEmail, shippingAddress, deliveryMethod, items, subtotal);
        _unitOfWork.Repository<Order>().Add(order);

        // Save to database
        var result = await _unitOfWork.Complete();

        if (result <= 0) return null;

        // Delete basket
        await _basketRepo.DeleteBasketAsync(basketId);

        // Return the Order
        return order;
    }

    public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
    {
        return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId, string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderdingSpecification(orderId, buyerEmail);

        return await _unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
    {
        var spec = new OrdersWithItemsAndOrderdingSpecification(buyerEmail);

        return await _unitOfWork.Repository<Order>().ListAsync(spec);
    }
}
