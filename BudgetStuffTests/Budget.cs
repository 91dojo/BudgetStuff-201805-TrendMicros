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

        public int DaysOfBudget()
        {
            var daysOfBudget = DateTime.DaysInMonth(FirstDay.Year, FirstDay.Month);
            return daysOfBudget;
        }
    }
}