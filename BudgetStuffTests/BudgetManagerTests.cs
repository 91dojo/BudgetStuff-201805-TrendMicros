using System;
using System.Collections.Generic;
using System.Linq;
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
            var startdate = new DateTime(2016, 2, 1);
            var enddate = new DateTime(2016, 1, 1);
            decimal expected = 0;

            var actual = _budgetmanager.TotalAmount(startdate, enddate);
        }

        [TestMethod]
        public void OneMonthNoBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 0}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 31), 0);
        }

        [TestMethod]
        public void OneMonthHasBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 3100}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 31), 3100);
        }

        [TestMethod]
        public void OnedayHasBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 3100}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 01), 100);
        }

        [TestMethod]
        public void TwodaysHasBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 3100}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 03, 02), 200);
        }

        [TestMethod]
        public void LeapYearFebHasBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201602", Amount = 29}
            );

            AmountShouldBe(new DateTime(2016, 02, 01), new DateTime(2016, 02, 15), 15);
        }

        private void GivenBudgets(params Budget[] budgets)
        {
            _repository.GetBudgets().ReturnsForAnyArgs(budgets.ToList());
        }

        [TestMethod]
        public void MultiMonth_NoBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 0},
                new Budget {YearMonth = "201704", Amount = 0}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 04, 30), 0);
        }

        [TestMethod]
        public void MultiMonth_Budget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 3100},
                new Budget {YearMonth = "201704", Amount = 30}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 04, 30), 3130);
        }

        [TestMethod]
        public void MultiMonth_1halfBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201701", Amount = 3100},
                new Budget {YearMonth = "201702", Amount = 28}
            );

            AmountShouldBe(new DateTime(2017, 01, 01), new DateTime(2017, 02, 15), 3115);
        }

        [TestMethod]
        public void MultiMonth_1Budget_1noBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 0},
                new Budget {YearMonth = "201704", Amount = 300}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 04, 30), 300);
        }

        [TestMethod]
        public void MultiMonth_1noBudget_1Budget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 310},
                new Budget {YearMonth = "201704", Amount = 0}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 04, 30), 310);
        }

        [TestMethod]
        public void MultiMonth_1Budget_1noBudget_1Budget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 3100},
                new Budget {YearMonth = "201704", Amount = 0},
                new Budget {YearMonth = "201705", Amount = 31}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 05, 31), 3131);
        }

        [TestMethod]
        public void MultiMonth_1noBudget_1Budget_1noBudget()
        {
            GivenBudgets(
                new Budget {YearMonth = "201703", Amount = 0},
                new Budget {YearMonth = "201704", Amount = 300},
                new Budget {YearMonth = "201705", Amount = 0}
            );

            AmountShouldBe(new DateTime(2017, 03, 01), new DateTime(2017, 05, 31), 300);
        }

        private void AmountShouldBe(DateTime startdate, DateTime enddate, decimal expected)
        {
            Assert.AreEqual(expected, _budgetmanager.TotalAmount(startdate, enddate));
        }

        private void GivenBudget(Dictionary<DateTime, Budget> mockBudget)
        {
            _repository.GetBudget(new DateTime(), new DateTime()).ReturnsForAnyArgs(mockBudget);
        }
    }
}