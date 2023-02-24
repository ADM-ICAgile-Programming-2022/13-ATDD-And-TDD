using System;
using LiftPassPricing.Application.Ports;

namespace LiftPassPricing.Domain
{
    public class Price
    {
        private readonly double baseCost;

        public Price(double baseCost)
        {
            this.baseCost = baseCost;
        }

        public double Calculate(LiftPassCommand liftPassCommand, bool isHoliday)
        {
            if (liftPassCommand.Age != null && liftPassCommand.Age < 6)
            {
                return 0;
            }

            return this.baseCost * CalculateReductionPercentage(liftPassCommand, isHoliday);
        }

        private double CalculateReductionPercentage(LiftPassCommand liftPassCommand, bool isHoliday)
        {
            if (liftPassCommand.Type != "night")
            {
                var reductionPercentage = DateReductionPercentage(liftPassCommand.Date, isHoliday);

                // TODO apply reduction for others
                if (liftPassCommand.Age != null && liftPassCommand.Age < 15)
                {
                    return .7;
                }
                else
                {
                    if (liftPassCommand.Age == null)
                    {
                        return reductionPercentage;
                    }
                    else
                    {
                        if (liftPassCommand.Age > 64)
                        {
                            return .75 * reductionPercentage;
                        }
                        else
                        {
                            return reductionPercentage;
                        }
                    }
                }
            }
            else
            {
                if (liftPassCommand.Age != null && liftPassCommand.Age >= 6)
                {
                    if (liftPassCommand.Age > 64)
                    {
                        return .4;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            return 1;

        }

        private double DateReductionPercentage(DateTime? date, bool isHoliday)
        {
            int reduction = 0;
            if (date != null)
            {
                if (!isHoliday && (int)date.Value.DayOfWeek == 1)
                {
                    reduction = 35;
                }
            }

            return 1 - reduction / 100.0;
        }
    }
}