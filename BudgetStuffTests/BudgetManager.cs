using System;
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

            return _repo.GetBudgets()
                .Sum(b => b.EffectiveAmount(period));
        }
    }
}