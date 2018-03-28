using System;

namespace BudgetStuffTests
{
    public class Budget
    {
        public int Amount { get; set; }

        public string YearMonth { get; set; }

        private DateTime FirstDay
        {
            get { return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null); }
        }

        private DateTime LastDay
        {
            get { return DateTime.ParseExact(YearMonth + DaysOfBudgetMonth(), "yyyyMMdd", null); }
        }

        public int EffectiveAmount(Period period)
        {
            return DailyAmount() * period.EffectiveDays(new Period(FirstDay, LastDay));
        }

        private int DailyAmount()
        {
            return Amount / DaysOfBudgetMonth();
        }

        private int DaysOfBudgetMonth()
        {
            var daysOfBudgetMonth = DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            return daysOfBudgetMonth;
        }
    }
}