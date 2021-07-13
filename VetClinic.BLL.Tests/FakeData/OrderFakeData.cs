using System.Collections.Generic;
using VetClinic.Core.Entities;
namespace VetClinic.BLL.Tests.FakeData
{
    public static class OrderFakeData
    {
        public static List<Order> GetEmployeeFakeData() =>
            new List<Order>
            {
                new Order
                {
                    Id = 1,
                    IsPaid = true,
                    OrderProcedureId = 1
                },
                new Order
                {
                    Id = 2,
                    IsPaid = true,
                    OrderProcedureId = 2
                },
                new Order
                {
                    Id = 3,
                    IsPaid = false,
                    OrderProcedureId = 3
                },
                new Order
                {
                    Id = 4,
                    IsPaid = false,
                    OrderProcedureId = 4
                },
                new Order
                {
                    Id = 5,
                    IsPaid = true,
                    OrderProcedureId = 5
                },
                new Order
                {
                    Id = 6,
                    IsPaid = false,
                    OrderProcedureId = 6
                },
                new Order
                {
                    Id = 7,
                    IsPaid = true,
                    OrderProcedureId = 7
                },
                new Order
                {
                    Id = 8,
                    IsPaid = false,
                    OrderProcedureId = 8
                },
                new Order
                {
                    Id = 9,
                    IsPaid = true,
                    OrderProcedureId = 9
                },
                new Order
                {
                    Id = 10,
                    IsPaid = false,
                    OrderProcedureId = 10
                }
            };
    }
}
