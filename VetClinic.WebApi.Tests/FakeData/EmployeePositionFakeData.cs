using System.Collections.Generic;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Tests.FakeData
{
    public static class EmployeePositionFakeData
    {
        public static List<EmployeePosition> GetEmployeePositionFakeData() =>
            new List<EmployeePosition>
            {
                new EmployeePosition
                {
                    Id = 1,
                    CurrentBaseSalary = 1000,
                    DismissedDate = null,
                    HierdDate = new System.DateTime(2021, 5, 12),
                    EmployeeId = "6fca381a-40d0-4bf9-a076-706e1a995662",
                    PositionId = 1,
                    Rate = 12
                },
                new EmployeePosition
                {
                    Id = 2,
                    CurrentBaseSalary = 10002,
                    DismissedDate = null,
                    HierdDate = new System.DateTime(2021, 5, 2),
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                    PositionId = 2,
                    Rate = 12
                },
                new EmployeePosition
                {
                    Id = 3,
                    CurrentBaseSalary = 1000,
                    DismissedDate = null,
                    HierdDate = new System.DateTime(2021, 5, 12),
                    EmployeeId = "804bbbca-3ffc-4d28-9b71-0d7788ddf681",
                    PositionId = 3,
                    Rate = 12
                },
                new EmployeePosition
                {
                    Id = 4,
                    CurrentBaseSalary = 10200,
                    DismissedDate = null,
                    HierdDate = new System.DateTime(2021, 3, 12),
                    EmployeeId = "0e216f00-f03b-4655-9f06-f166828d35df",
                    PositionId = 4,
                    Rate = 12
                },
                new EmployeePosition
                {
                    Id = 5,
                    CurrentBaseSalary = 12000,
                    DismissedDate = null,
                    HierdDate = new System.DateTime(2021, 2, 12),
                    EmployeeId = "ada82298-b807-4267-a9a2-c3955e975294",
                    PositionId = 5,
                    Rate = 12
                }
            };
    }
}
