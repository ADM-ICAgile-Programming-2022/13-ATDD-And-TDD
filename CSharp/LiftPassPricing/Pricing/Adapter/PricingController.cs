using System;
using System.Globalization;
using LiftPassPricing.Application.Ports;
using Nancy;

namespace LiftPassPricing.Adapter
{
    public class PricingController
    {
        private readonly ICreateBasePriceUseCase createBasePriceUseCase;
        private readonly ICalculatePriceUseCase calculatePriceUseCase;

        public PricingController(ICreateBasePriceUseCase createBasePriceUseCase, ICalculatePriceUseCase calculatePriceUseCase)
        {
            this.createBasePriceUseCase = createBasePriceUseCase;
            this.calculatePriceUseCase = calculatePriceUseCase;
        }

        public object PutPricingRequest(Request request)
        {

            int cost = int.Parse(request.Query["cost"]);
            string type = request.Query["type"];

            this.createBasePriceUseCase.CreateBasePrice(cost, type);

            return "";
        }

        public string GetPricingRequest(Request request)
        {
            int? age = request.Query["age"] != null ? int.Parse(request.Query["age"]) : null;
            string typeValue = request.Query["type"];
            string date = request.Query["date"];

            var pass = new LiftPassCommand
            {
                Age = age,
                Type = typeValue,
                Date = date != null
                    ? DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture)
                    : (DateTime?)null
            };

            var cost = this.calculatePriceUseCase.CalculatePrice(pass);

            return FormatCostJson(cost);
        }

        private string FormatCostJson(double cost)
        {
            return "{ \"cost\": " + (int)Math.Ceiling(cost) + "}";
        }
    }

}
