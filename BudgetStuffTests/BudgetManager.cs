using System;
using System.Collections.Generic;

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

            var budgetMap = _repo.GetBudget(startDate, endDate);
            if (IsOnlyOneBudget(budgetMap))
            {
                var effectiveDays = EffectiveDays(period);
                return GetAmount(DateTime.DaysInMonth(startDate.Year, startDate.Month), budgetMap[startDate].amount,
                    effectiveDays);
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
                    amount += GetAmount(DateTime.DaysInMonth(month.Year, month.Month), budgetMap[month].amount,
                        timeSpan);
                    index++;
                }
                return amount;
            }
        }

        private static int EffectiveDays(Period period)
        {
            var effectiveDays = (period.EndDate - period.StartDate).Days + 1;
            return effectiveDays;
        }

        private static bool IsOnlyOneBudget(Dictionary<DateTime, Budget> budgetMap)
        {
            return budgetMap.Keys.Count == 1;
        }

        private static decimal GetAmount(int monthdays, int amount, int actualdays)
        {
            return amount / monthdays * actualdays;
            //return BudgetMap[startdate].amount / DateTime.DaysInMonth(startdate.Year, startdate.Month) * (timeSpan.Days + 1);
        }

        // private decimal GetBudget
    }
}