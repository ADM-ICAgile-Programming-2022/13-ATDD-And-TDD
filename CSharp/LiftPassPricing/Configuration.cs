using LiftPassPricing.Adapter;
using LiftPassPricing.Application;

namespace LiftPassPricing
{
    public class Configuration
    {
        public PricingController PricingControler { get; }

        public Configuration()
        {
            var dataRepository = new MySqlDataRepository(@"Database=lift_pass;Data Source=localhost;User Id=root");
            var pricingService = new PriceService(dataRepository);
            this.PricingControler =  new PricingController(pricingService, pricingService);
        }

    }
}
