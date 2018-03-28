using System;
using System.Collections.Generic;

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
            if (startDate > endDate)
                throw new InvalidException();

            var budgetMap = _repo.GetBudget(startDate, endDate);
            if (IsOnlyOneMonth(budgetMap))
            {
                var daysOfPeriod = DaysOfPeriod(startDate, endDate);
                return GetAmount(DateTime.DaysInMonth(startDate.Year, startDate.Month), budgetMap[startDate].amount,
                    daysOfPeriod);
            }
            else
            {
                decimal amount = 0;
                int index = 0;
                foreach (var month in budgetMap.Keys)
                {
                    int timeSpan = 0;
                    if (IsFirstMonth(index))
                    {
                        timeSpan = DateTime.DaysInMonth(month.Year, month.Month) - startDate.Day + 1;
                    }
                    else if (IsLastMonth(index, budgetMap))
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

        private static bool IsOnlyOneMonth(Dictionary<DateTime, Budget> budgetMap)
        {
            return budgetMap.Keys.Count == 1;
        }

        private static int DaysOfPeriod(DateTime startDate, DateTime endDate)
        {
            var daysOfPeriod = (endDate - startDate).Days + 1;
            return daysOfPeriod;
        }

        private static decimal GetAmount(int monthdays, int amount, int actualdays)
        {
            return amount / monthdays * actualdays;
            //return BudgetMap[startdate].amount / DateTime.DaysInMonth(startdate.Year, startdate.Month) * (timeSpan.Days + 1);
        }

        private static bool IsFirstMonth(int index)
        {
            return index == 0;
        }

        private static bool IsLastMonth(int index, Dictionary<DateTime, Budget> budgetMap)
        {
            return index == budgetMap.Keys.Count - 1;
        }

        // private decimal GetBudget
    }
}