using System;
using System.Collections.Generic;
using VetClinic.Core.Entities;
namespace VetClinic.BLL.Tests.FakeData
{
    public static class ProcedureFakeData
    {
        public static List<Procedure> GetProcedureFakeData() =>
            new List<Procedure>
            {
                new Procedure
                {
                    Id = 1,
                    Title = "Procedure 1",
                    Description = "Surgical procedure.",
                    Duration = new TimeSpan(hours:0, minutes:30, seconds:0),
                    Price = 1500
                },
                new Procedure
                {
                    Id = 2,
                    Title = "Procedure 2",
                    Description = "Examination.",
                    Duration = new TimeSpan(hours:0, minutes:5, seconds:0),
                    Price = 300
                },
                new Procedure
                {
                    Id = 3,
                    Title = "Procedure 3",
                    Description = "Surgical procedure.",
                    Duration = new TimeSpan(hours:1, minutes:30, seconds:0),
                    Price = 2000
                },
                new Procedure
                {
                    Id = 4,
                    Title = "Procedure 4",
                    Description = "Examination.",
                    Duration = new TimeSpan(hours:0, minutes:10, seconds:0),
                    Price = 1000
                },
                new Procedure
                {
                    Id = 5,
                    Title = "Procedure 5",
                    Description = "Surgical procedure.",
                    Duration = new TimeSpan(hours:1, minutes:10, seconds:0),
                    Price = 1650
                },
                new Procedure
                {
                    Id = 6,
                    Title = "Procedure 6",
                    Description = "Examination.",
                    Duration = new TimeSpan(hours:0, minutes:1, seconds:30),
                    Price = 100
                },
                new Procedure
                {
                    Id = 7,
                    Title = "Procedure 7",
                    Description = "Surgical procedure.",
                    Duration = new TimeSpan(hours:0, minutes:25, seconds:0),
                    Price = 500
                },
                new Procedure
                {
                    Id = 8,
                    Title = "Procedure 8",
                    Description = "Surgical procedure.",
                    Duration = new TimeSpan(hours:0, minutes:50, seconds:0),
                    Price = 850
                },
                new Procedure
                {
                    Id = 9,
                    Title = "Procedure 9",
                    Description = "Examination.",
                    Duration = new TimeSpan(hours:0, minutes:15, seconds:0),
                    Price = 1150
                },
                new Procedure
                {
                    Id = 10,
                    Title = "Procedure 10",
                    Description = "Examination.",
                    Duration = new TimeSpan(hours:0, minutes:5, seconds:0),
                    Price = 900
                },
            };
    }
}
