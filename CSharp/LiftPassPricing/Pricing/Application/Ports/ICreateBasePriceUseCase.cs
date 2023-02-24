namespace LiftPassPricing.Application.Ports
{
    public interface ICreateBasePriceUseCase
    {
        void CreateBasePrice(int cost, string type);
    }
}