using System.Collections.Generic;
using VetClinic.Core.Entities;

namespace VetClinic.BLL.Tests.FakeData
{
    public static class PositionFakeData
    {
        public static List<Position> GetPositionFakeData() =>
            new List<Position>
            {
                new Position
                {
                    Id = 1,
                    Title = "Doctor",
                },
                new Position
                {
                    Id = 2,
                    Title = "Anesthetist",
                },
                new Position
                {
                    Id = 3,
                    Title = "Accountant",
                },
                new Position
                {
                    Id = 4,
                    Title = "Admin",
                }
            };
    }
}
