using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BudgetStuffTests
{
    [TestClass]
    public class BudgetManagerTests
    {
        private IRepository<Budget> _repository = Substitute.For<IRepository<Budget>>();
        private BudgetManager _budgetmanager;

        [TestInitializeAttribute]
        public void TestInit()
        {
            _budgetmanager = new BudgetManager(_repository);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidException))]
        public void InvalidDate()
        {
            var startDate = new DateTime(2017, 2, 1);
            var endDate = new DateTime(2017, 1, 1);

            var actual = _budgetmanager.TotalAmount(startDate, endDate);
        }

        [TestMethod]
        public void one_budget_which_amount_is_0()
        {
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 0}
                }
            });

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 31), 0);
        }

        [TestMethod]
        public void period_inside_one_budget_month()
        {
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 31), 3100);
        }

        [TestMethod]
        public void one_effective_day_period_inside_one_budget()
        {
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 01), 100);
        }

        [TestMethod]
        public void two_effective_days_which_period_inside_one_budget_month()
        {
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 02), 200);
        }

        [TestMethod]
        public void LeapYearFebHasBudget()
        {
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2016, 02, 01),
                    new Budget() {amount = 29}
                }
            });

            AmountShouldBe(new DateTime(2016, 02, 01), new DateTime(2016, 02, 15), 15);
        }

        [TestMethod]
        public void multiple_budgets_which_amount_are_both_0()
        {
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017, 04, 01),
                    new Budget() {amount = 0}
                }
            });

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 04, 30), 0);
        }

        [TestMethod]
        public void multiple_budgets_with_amount()
        {
            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 3100}
                },
                {
                    new DateTime(2017, 04, 01),
                    new Budget() {amount = 30}
                }
            });

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 04, 30), 3130);
        }

        [TestMethod]
        public void multiple_budgets_which_period_partial_overlap_last_budget_month()
        {
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 01, 01),
                    new Budget() {amount = 3100}
                },
                {
                    new DateTime(2017, 02, 01),
                    new Budget() {amount = 28}
                }
            });

            AmountShouldBe(new DateTime(2017, 01, 01), new DateTime(2017, 02, 15), 3115);
        }

        [TestMethod]
        public void MultiMonth_1Budget_1noBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 04, 30);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017, 04, 01),
                    new Budget() {amount = 300}
                }
            });

            AmountShouldBe(startdate, enddate, 300);
        }

        [TestMethod]
        public void MultiMonth_1noBudget_1Budget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 04, 30);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 310}
                },
                {
                    new DateTime(2017, 04, 01),
                    new Budget() {amount = 0}
                }
            });

            AmountShouldBe(startdate, enddate, 310);
        }

        [TestMethod]
        public void MultiMonth_1Budget_1noBudget_1Budget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 05, 31);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 3100}
                },
                {
                    new DateTime(2017, 04, 01),
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017, 05, 01),
                    new Budget() {amount = 31}
                }
            });

            AmountShouldBe(startdate, enddate, 3131);
        }

        [TestMethod]
        public void MultiMonth_1noBudget_1Budget_1noBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 05, 31);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017, 03, 01),
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017, 04, 01),
                    new Budget() {amount = 300}
                },
                {
                    new DateTime(2017, 05, 01),
                    new Budget() {amount = 0}
                }
            });

            AmountShouldBe(startdate, enddate, 300);
        }

        private void AmountShouldBe(DateTime startdate, DateTime enddate, decimal expected)
        {
            Assert.AreEqual(expected, _budgetmanager.TotalAmount(startdate, enddate));
        }

        private void GivenBudget(Dictionary<DateTime, Budget> budgets)
        {
            _repository.GetBudget(new DateTime(), new DateTime()).ReturnsForAnyArgs(budgets);
        }
    }
}