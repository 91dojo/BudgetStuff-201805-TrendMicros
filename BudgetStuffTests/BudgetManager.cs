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

            return budgets.Sum(
                budget => budget.EffectiveAmount(period));
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
    }
}