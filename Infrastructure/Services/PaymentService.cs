using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Terminal;
using Product = Core.Entities.Product;

namespace Infrastructure.Services;
public class PaymentService : IPaymentService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public PaymentService(IBasketRepository basketRepository, 
        IUnitOfWork unitOfWork, 
        IConfiguration configuration)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
    {
        // Set the Stripe API key to use for the request
        StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

        // Get the customer basket from the repository based on the provided basketId
        var basket = await _basketRepository.GetBasketAsync(basketId);

        // Initialize a variable to store the shipping price
        var shippingPrice = 0m;

        // If the basket has a selected delivery method, retrieve its price
        if (basket.DeliveryMethodId.HasValue)
        {
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>()
                .GetByIdAsync((int)basket.DeliveryMethodId);
            shippingPrice = deliveryMethod.Price;
        }

        // Update the prices of the items in the basket to match the latest prices from the database
        foreach (var item in basket.Items)
        {
            var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

            // If the price in the basket doesn't match the price in the database, update it
            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }

        // Create a new PaymentIntent or update an existing one based on the basket's payment status

        // Create a PaymentIntentService instance to interact with Stripe API
        var service = new PaymentIntentService();
        PaymentIntent intent;

        // If the basket doesn't have a PaymentIntentId, create a new PaymentIntent
        if (string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            // Calculate the total amount for the PaymentIntent, including item prices and shipping price
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = new List<string> { "card" }
            };

            // Create the PaymentIntent and store its ID and client secret in the basket
            intent = await service.CreateAsync(options);
            basket.PaymentIntentId = intent.Id;
            basket.ClientSecret = intent.ClientSecret;
        }
        else
        {
            // If the basket already has a PaymentIntentId, update the existing PaymentIntent
            var options = new PaymentIntentUpdateOptions
            {
                Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)shippingPrice * 100
            };
            await service.UpdateAsync(basket.PaymentIntentId, options);
        }

        // Update the basket with the new PaymentIntent information
        await _basketRepository.UpdateBasketAsync(basket);

        // Return the updated customer basket
        return basket;
    }

}
