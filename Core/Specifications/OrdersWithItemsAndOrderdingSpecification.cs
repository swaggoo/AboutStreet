using Core.Entities.OrderAggregate;
using System.Linq.Expressions;

namespace Core.Specifications;
public class OrdersWithItemsAndOrderdingSpecification : BaseSpecification<Order>
{
    public OrdersWithItemsAndOrderdingSpecification(string email) : base(o => o.BuyerEmail == email)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
        AddOrderByDescending(o => o.OrderTime);
    }

    public OrdersWithItemsAndOrderdingSpecification(int id, string email)
        : base(o => o.Id == id && o.BuyerEmail == email)
    {
        AddInclude(o => o.OrderItems);
        AddInclude(o => o.DeliveryMethod);
    }
}
