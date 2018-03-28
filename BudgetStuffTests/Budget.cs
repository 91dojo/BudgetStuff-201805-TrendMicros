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

        public int DaysOfBudgetMonth()
        {
            var daysOfBudgetMonth = DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            return daysOfBudgetMonth;
        }
    }
}