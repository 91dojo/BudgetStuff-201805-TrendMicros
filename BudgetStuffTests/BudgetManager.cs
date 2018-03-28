using System;
using System.Collections.Generic;
using System.Linq;

namespace BudgetStuffTests
{
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
            decimal totalAmount = 0;
            foreach (var budget in budgets)
            {
                var effectiveDays = EffectiveDays(period, budget);

                totalAmount += EffectiveAmount(
                    budget.DaysOfBudgetMonth(),
                    budget.Amount,
                    effectiveDays);
            }
            return totalAmount;
            int index = 0;
            foreach (var month in budgetMap.Keys)
            {
                var effectiveDays = EffectiveDays(period, budgets[index]);

                totalAmount += EffectiveAmount(DateTime.DaysInMonth(month.Year, month.Month),
                    budgetMap[month].Amount,
                    effectiveDays);
                index++;
            }
            return totalAmount;
        }

        private static int EffectiveDays(Period period, Budget budget)
        {
            var startDate = period.StartDate < budget.FirstDay
                ? budget.FirstDay
                : period.StartDate;


            var endDate = period.EndDate > budget.LastDay
                ? budget.LastDay
                : period.EndDate;

            return (int) (endDate.AddDays(1) - startDate).TotalDays;
        }

        private static decimal EffectiveAmount(Period period, Budget budget)
        {
            var effectiveDays = period.Days();

            return budget.DailyAmount() * effectiveDays;
        }

        private static bool OnlyOneBudget(List<Budget> budgets)
        {
            return budgets.Count == 1;
        }

        private static decimal EffectiveAmount(int daysOfBudgetMonth, int amountOfBudget, int effectiveDays)
        {
            return amountOfBudget / daysOfBudgetMonth * effectiveDays;
        }

        private static bool IsFirstMonth(int index)
        {
            return index == 0;
        }

        private static bool IsLastMonth(int index, List<Budget> budgets)
        {
            return index == budgets.Count - 1;
        }

        // private decimal GetBudget
    }
}