using System;
using System.Collections.Generic;
using System.Linq;

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

        public int EffectiveDays()
        {
            var effectiveDays = (EndDate - StartDate).Days + 1;
            return effectiveDays;
        }
    }

    public class BudgetManager
    {
        private readonly IRepository<Budget> _repo;

        public BudgetManager(IRepository<Budget> repo)
        {
            _repo = repo;
        }

        public decimal TotalAmount(DateTime startDate, DateTime endDate)
        {
            var period = new Period(startDate, endDate);

            var budgets = _repo.GetBudgets();
            var budgetMap = budgets.ToDictionary(x => x.FirstDay, x => x);

            if (IsOnlyOneBudget(budgetMap))
            {
                //TODO: 改成從 budget
                var daysOfBudget = DateTime.DaysInMonth(startDate.Year, startDate.Month);

                return GetEffectiveAmount(daysOfBudget,
                    budgetMap[startDate].Amount,
                    period.EffectiveDays());
            }
            else
            {
                decimal amount = 0;
                int index = 0;
                foreach (var month in budgetMap.Keys)
                {
                    int timeSpan = 0;
                    if (index == 0)
                    {
                        timeSpan = DateTime.DaysInMonth(month.Year, month.Month) - startDate.Day + 1;
                    }
                    else if (index == budgetMap.Keys.Count - 1)
                    {
                        timeSpan = endDate.Day;
                    }
                    else
                    {
                        timeSpan = DateTime.DaysInMonth(month.Year, month.Month);
                    }
                    amount += GetEffectiveAmount(DateTime.DaysInMonth(month.Year, month.Month), budgetMap[month].Amount,
                        timeSpan);
                    index++;
                }
                return amount;
            }
        }

        private static bool IsOnlyOneBudget(Dictionary<DateTime, Budget> budgetMap)
        {
            return budgetMap.Keys.Count == 1;
        }

        private static decimal GetEffectiveAmount(int daysOfBudget, int amountOfBudget, int effectiveDays)
        {
            return amountOfBudget / daysOfBudget * effectiveDays;
        }
    }
}