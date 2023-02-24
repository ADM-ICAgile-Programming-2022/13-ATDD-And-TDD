using LiftPassPricing.Adapter;
using LiftPassPricing.Application;

namespace LiftPassPricing
{
    public class TestConfiguration
    {
        public PricingController PricingControler { get; }

        public TestConfiguration()
        {
            var dataRepository = new SqlLightDataRepository();
            dataRepository.InitDatabase();

            var pricingService = new PriceService(dataRepository);
            this.PricingControler =  new PricingController(pricingService, pricingService);
        }

    }
}
