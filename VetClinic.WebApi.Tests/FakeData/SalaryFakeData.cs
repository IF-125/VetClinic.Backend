using System.Collections.Generic;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Tests.FakeData
{
    public static class SalaryFakeData
    {
        public static List<Salary> GetFakeSalaryData() =>
            new List<Salary>
            {
                new Salary
                {
                    Id = 1,
                    Amount = 1000,
                    Bonus = 10,
                    Date = new System.DateTime(2021, 8, 6),
                    EmployeePositionId = 1
                },
                new Salary
                {
                    Id = 2,
                    Amount = 10100,
                    Bonus = 130,
                    Date = new System.DateTime(2021, 8, 6),
                    EmployeePositionId = 2
                },
                new Salary
                {
                    Id = 3,
                    Amount = 10500,
                    Bonus = 10,
                    Date = new System.DateTime(2021, 8, 6),
                    EmployeePositionId = 2
                },
                new Salary
                {
                    Id = 4,
                    Amount = 10000,
                    Bonus = 10,
                    Date = new System.DateTime(2021, 8, 6),
                    EmployeePositionId = 2
                },
                new Salary
                {
                    Id = 5,
                    Amount = 100230,
                    Bonus = 10,
                    Date = new System.DateTime(2021, 8, 6),
                    EmployeePositionId = 5
                },

            };
    }
}
