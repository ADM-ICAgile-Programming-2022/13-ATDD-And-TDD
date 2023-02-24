namespace LiftPassPricing.Application.Ports
{
    public interface ICalculatePriceUseCase
    {
        double CalculatePrice(LiftPassCommand liftPassCommand);
    }
}