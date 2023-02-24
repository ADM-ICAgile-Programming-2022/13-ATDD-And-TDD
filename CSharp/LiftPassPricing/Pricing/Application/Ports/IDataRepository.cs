using System;
using LiftPassPricing.Domain;

namespace LiftPassPricing.Application.Ports
{
    public interface IDataRepository
    {
        void InsertPrice(int basePriceCost, string basePriceType);

        bool IsHoliday(DateTime d);
        Price ReadPrice(string liftPassType);
    }
}
