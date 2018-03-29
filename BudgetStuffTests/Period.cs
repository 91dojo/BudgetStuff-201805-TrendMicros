using System;

namespace BudgetStuffTests
{
    public class Period
    {
        public Period(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new InvalidException();

            StartDate = startDate;
            EndDate = endDate;
        }

        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }


        public int EffectiveDays(Period period)
        {
            var effectiveEndDate = EndDate > period.EndDate
                ? period.EndDate
                : EndDate;

            var effectiveStartDate = StartDate < period.StartDate
                ? period.StartDate
                : StartDate;

            return (int) (effectiveEndDate.AddDays(1) - effectiveStartDate).TotalDays;
        }
    }
}