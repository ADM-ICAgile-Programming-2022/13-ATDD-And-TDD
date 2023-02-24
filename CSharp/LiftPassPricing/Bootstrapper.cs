using LiftPassPricing.Adapter;
using Nancy;
using Nancy.TinyIoc;

namespace LiftPassPricing
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly Configuration configuration;

        public Bootstrapper()
        {
        }

        public Bootstrapper(Configuration configuration)
        {
            this.configuration = configuration;
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register<PricingController>(configuration.PricingControler);
        }
    }
}
