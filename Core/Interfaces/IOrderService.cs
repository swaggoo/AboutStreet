﻿using Core.Entities.OrderAggregate;

namespace Core.Interfaces;
public interface IOrderService
{
    Task<Order> CreateOrderAsync(
        string buyerEmail, 
        int deliveryMethod, 
        string basketId, 
        Address ShippingAddress);
    Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);
    Task<Order> GetOrderByIdAsync(int orderId, string buyerEmail);
    Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
}
