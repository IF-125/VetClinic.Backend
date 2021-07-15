using System;
using System.Collections.Generic;
using VetClinic.Core.Entities;
namespace VetClinic.BLL.Tests.FakeData
{
    public static class OrderFakeData
    {
        public static List<Order> GetOrderFakeData() =>
            new List<Order>
            {
                new Order
                {
                    Id = 1,
                    IsPaid = true,
                    OrderProcedureId = 1,
                    CreatedAt = new DateTime(2021, 6, 12)
                },
                new Order
                {
                    Id = 2,
                    IsPaid = true,
                    OrderProcedureId = 2,
                    CreatedAt = new DateTime(2020, 4, 12)
                },
                new Order
                {
                    Id = 3,
                    IsPaid = false,
                    OrderProcedureId = 3,
                    CreatedAt = new DateTime(2021, 7, 18)
                },
                new Order
                {
                    Id = 4,
                    IsPaid = false,
                    OrderProcedureId = 4,
                    CreatedAt = new DateTime(2020, 10, 16)
                },
                new Order
                {
                    Id = 5,
                    IsPaid = true,
                    OrderProcedureId = 5,
                    CreatedAt = new DateTime(2021, 6, 23)
                },
                new Order
                {
                    Id = 6,
                    IsPaid = true,
                    OrderProcedureId = 6,
                    CreatedAt = new DateTime(2021, 1, 25)
                },
                new Order
                {
                    Id = 7,
                    IsPaid = true,
                    OrderProcedureId = 7,
                    CreatedAt = new DateTime(2020, 1, 10)
                },
                new Order
                {
                    Id = 8,
                    IsPaid = false,
                    OrderProcedureId = 8,
                    CreatedAt = new DateTime(2021, 5, 13)
                },
                new Order
                {
                    Id = 9,
                    IsPaid = true,
                    OrderProcedureId = 9,
                    CreatedAt = new DateTime(2020, 6, 19)
                },
                new Order
                {
                    Id = 10,
                    IsPaid = false,
                    OrderProcedureId = 10,
                    CreatedAt = new DateTime(2021, 4, 17)
                }
            };
    }
}
