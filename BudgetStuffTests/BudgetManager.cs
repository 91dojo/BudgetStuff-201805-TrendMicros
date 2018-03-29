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

            if (IsOnlyOneMonth(budgets))
            {
                //TODO: 改成從 budget
                var budget = budgets[0];

                return GetEffectiveAmount(budget.Days(), budget.Amount, period.EffectiveDays());
            }
            else
            {
                decimal totalAmount = 0;
                int index = 0;
                foreach (var month in budgetMap.Keys)
                {
                    var effectiveDays = EffectiveDays(period, index, budgets);

                    totalAmount += GetEffectiveAmount(DateTime.DaysInMonth(month.Year, month.Month),
                        budgetMap[month].Amount,
                        effectiveDays);
                    index++;
                }
                return totalAmount;
            }
        }

        private static int EffectiveDays(Period period, int index, List<Budget> budgets)
        {
            var month = budgets[index].FirstDay;
            int effectiveDays = 0;
            if (IsFirstBudget(index))
            {
                effectiveDays = DateTime.DaysInMonth(month.Year, month.Month) - period.StartDate.Day + 1;
            }
            else if (IsLastBudget(index, budgets))
            {
                effectiveDays = period.EndDate.Day;
            }
            else
            {
                effectiveDays = DateTime.DaysInMonth(month.Year, month.Month);
            }
            return effectiveDays;
        }

        private static bool IsLastBudget(int index, List<Budget> budgets)
        {
            return index == budgets.Count - 1;
        }

        private static bool IsFirstBudget(int index)
        {
            return index == 0;
        }

        private static bool IsOnlyOneMonth(List<Budget> budgets)
        {
            return budgets.Count == 1;
        }

        private static decimal GetEffectiveAmount(int daysOfBudget, int amountOfBudget, int effectiveDays)
        {
            return amountOfBudget / daysOfBudget * effectiveDays;
        }
    }
}