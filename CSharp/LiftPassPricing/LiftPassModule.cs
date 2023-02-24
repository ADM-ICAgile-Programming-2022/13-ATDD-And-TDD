using LiftPassPricing.Adapter;
using Nancy;

namespace LiftPassPricing
{
    public class LiftPassModule : NancyModule
    {
        public LiftPassModule(PricingController pricingController)
        {
            base.Put("/prices", _ => pricingController.PutPricingRequest(this.Request));

            base.Get("/prices", _ => pricingController.GetPricingRequest(this.Request));

            After += ctx =>
            {
                ctx.Response.ContentType = "application/json";
            };

        }

    }
}
