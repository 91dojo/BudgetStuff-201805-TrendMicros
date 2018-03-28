﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BudgetStuffTests
{
    [TestClass]
    public class UnitTest1
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
            // arrange
            var startdate = DateTime.Today;
            var enddate = DateTime.Now.AddDays(-1);
            //var budgetmanager = new BudgetManager();
            decimal expected = 0;
            
            //act
            var actual = _budgetmanager.TotalAmount(startdate, enddate);
        }

        [TestMethod]
        public void OneMonthNoBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 31);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017,03,01),
                    new Budget() {amount = 0}
                }
            });
            
            AmountShouldBe(startdate, enddate, 0);
        }

        [TestMethod]
        public void OneMonthHasBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 31);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017,03,01),
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(startdate, enddate, 3100);
        }

        [TestMethod]
        public void OnedayHasBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 01);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017,03,01),
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(startdate, enddate, 100);
        }

        [TestMethod]
        public void TwodaysHasBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 03, 02);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017,03,01),
                    new Budget() {amount = 3100}
                }
            });

            AmountShouldBe(startdate, enddate, 200);
        }

        [TestMethod]
        public void LeapYearFebHasBudget()
        {
            // arrange
            var startdate = new DateTime(2016, 02, 01);
            var enddate = new DateTime(2016, 02, 15);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2016,02,01),
                    new Budget() {amount = 29}
                }
            });

            AmountShouldBe(startdate, enddate, 15);
        }

        [TestMethod]
        public void MultiMonth_NoBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 04, 30);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017,03,01),
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017,04,01), 
                    new Budget() {amount = 0}
                }
            });

            AmountShouldBe(startdate, enddate, 0);
        }

        [TestMethod]
        public void MultiMonth_Budget()
        {
            // arrange
            var startdate = new DateTime(2017, 03, 01);
            var enddate = new DateTime(2017, 04, 30);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017,03,01),
                    new Budget() {amount = 3100}
                },
                {
                    new DateTime(2017,04,01),
                    new Budget() {amount = 30}
                }
            });

            AmountShouldBe(startdate, enddate, 3130);
        }

        [TestMethod]
        public void MultiMonth_1halfBudget()
        {
            // arrange
            var startdate = new DateTime(2017, 01, 01);
            var enddate = new DateTime(2017, 02, 15);

            // mock
            GivenBudget(new Dictionary<DateTime, Budget>()
            {
                {
                    new DateTime(2017,01,01),
                    new Budget() {amount = 3100}
                },
                {
                    new DateTime(2017,02,01),
                    new Budget() {amount = 28}
                }
            });

            AmountShouldBe(startdate, enddate, 3115);
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
                    new DateTime(2017,03,01),
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017,04,01),
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
                    new DateTime(2017,03,01),
                    new Budget() {amount = 310}
                },
                {
                    new DateTime(2017,04,01),
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
                    new DateTime(2017,03,01),
                    new Budget() {amount = 3100}
                },
                {
                    new DateTime(2017,04,01),
                    new Budget() {amount = 0}
                },
                {
                new DateTime(2017,05,01),
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
                    new DateTime(2017,03,01),
                    new Budget() {amount = 0}
                },
                {
                    new DateTime(2017,04,01),
                    new Budget() {amount = 300}
                },
                {
                    new DateTime(2017,05,01),
                    new Budget() {amount = 0}
                }
            });

            AmountShouldBe(startdate, enddate, 300);
        }

        private void AmountShouldBe(DateTime startdate, DateTime enddate, decimal expected)
        {
            Assert.AreEqual(expected,_budgetmanager.TotalAmount(startdate, enddate));
        }

        private void GivenBudget(Dictionary<DateTime, Budget> mockBudget)
        {
            _repository.GetBudget(new DateTime(), new DateTime()).ReturnsForAnyArgs(mockBudget);
        }
    }


}