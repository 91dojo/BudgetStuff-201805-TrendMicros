using System;
using System.Collections.Generic;

namespace BudgetStuffTests
{
    public interface IRepository<T>
    {
        Dictionary<DateTime, Budget> GetBudget(DateTime starDateTime, DateTime endDateTime);
        List<Budget> GetBudgets();
    }
}