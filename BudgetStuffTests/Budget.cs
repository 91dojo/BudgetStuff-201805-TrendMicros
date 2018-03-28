using System;

namespace BudgetStuffTests
{
    public class Budget
    {
        public int Amount { get; set; }

        public DateTime FirstDay
        {
            get { return DateTime.ParseExact(YearMonth + "01", "yyyyMMdd", null); }
        }

        public string YearMonth { get; set; }

        public DateTime LastDay
        {
            get
            {
                return DateTime.ParseExact(YearMonth + DaysOfBudgetMonth(), "yyyyMMdd", null);
            }
        }

        public int DaysOfBudgetMonth()
        {
            var daysOfBudgetMonth = DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            return daysOfBudgetMonth;
        }

        public int DailyAmount()
        {
            return Amount / DaysOfBudgetMonth();
        }
    }
}