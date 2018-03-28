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
            return budgets.Sum(b => EffectiveAmount(b, period));
            decimal totalAmount = 0;
            foreach (var budget in budgets)
            {
                totalAmount += EffectiveAmount(budget, period);
            }
            return totalAmount;
        }

        private static int EffectiveAmount(Budget budget, Period period)
        {
            return budget.DailyAmount() * period.EffectiveDays(budget);
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