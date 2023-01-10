using ShoppingCart.Base.Repositories;
using ShoppingCart.Repository;
using ShoppingCart.Service;

namespace shopping_cart.Extension
{
    public static class ServiceExtension
    {
        public static object Configuration { get; private set; }
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDiscountRepository, DiscountRepository>();
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<DiscountService, DiscountService>();
        }
    }
}
