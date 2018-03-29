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

            return budgets.Sum(
                budget => EffectiveAmount(budget, period));
        }

        private static decimal EffectiveAmount(Budget budget, Period period)
        {
            return (decimal) (DailyAmount(budget) * EffectiveDays(period, budget));
        }

        private static int DailyAmount(Budget budget)
        {
            return budget.Amount / budget.Days();
        }

        private static int EffectiveDays(Period period, Budget budget1)
        {
            var effectiveEndDate = period.EndDate;
            if (period.EndDate > budget1.LastDay)
            {
                effectiveEndDate = budget1.LastDay;
            }

            var effectiveStartDate = period.StartDate;
            if (period.StartDate < budget1.FirstDay)
            {
                effectiveStartDate = budget1.FirstDay;
            }

            return (int) (effectiveEndDate.AddDays(1) - effectiveStartDate).TotalDays;
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