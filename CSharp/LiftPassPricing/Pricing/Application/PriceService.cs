using LiftPassPricing.Application.Ports;

namespace LiftPassPricing.Application
{
    public class PriceService : ICreateBasePriceUseCase, ICalculatePriceUseCase
    {
        private readonly IDataRepository dataRepository;

        public PriceService(IDataRepository dataRepository)
        {
            this.dataRepository = dataRepository;
        }

        public void CreateBasePrice(int cost, string type)
        {
            this.dataRepository.InsertPrice(cost, type);
        }

        public double CalculatePrice(LiftPassCommand liftPassCommand)
        {
            var price = this.dataRepository.ReadPrice(liftPassCommand.Type);

            var isHoliday = liftPassCommand.Date.HasValue && this.dataRepository.IsHoliday(liftPassCommand.Date.Value);

            return price.Calculate(liftPassCommand, isHoliday);
        }
    }
}
