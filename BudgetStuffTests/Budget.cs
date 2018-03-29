using System;

namespace BudgetStuffTests
{
    public class Budget
    {
        public string YearMonth { get; set; }
        public int Amount { get; set; }

        public DateTime FirstDay
        {
            get { return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null); }
        }

        public DateTime LastDay
        {
            get { return DateTime.ParseExact(YearMonth + Days(), "yyyyMMdd", null); }
        }

        public int Days()
        {
            return DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
        }

        public int DailyAmount()
        {
            return Amount / Days();
        }

        public decimal EffectiveAmount(Period period)
        {
            return (decimal) (DailyAmount() * period.OverlappingDays(new Period(this.FirstDay, this.LastDay)));
        }
    }
}