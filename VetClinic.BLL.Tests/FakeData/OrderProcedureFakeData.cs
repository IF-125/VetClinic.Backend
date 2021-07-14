using System;
using System.Collections.Generic;
using VetClinic.Core.Entities;
namespace VetClinic.BLL.Tests.FakeData
{
    public static class OrderProcedureFakeData
    {
        public static List<OrderProcedure> GetOrderProcedureFakeData() =>
            new List<OrderProcedure>
            {
                new OrderProcedure
                {
                    Id = 1,
                    Count = 1,
                    Time = new TimeSpan(hours:1, minutes:12, seconds:0),
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 1,
                    AppointmentId = 1,
                    ProcedureId = 4,
                    PetId = 3,
                    EmployeeId = 1
                },
                new OrderProcedure
                {
                    Id = 2,
                    Count = 1,
                    Time = new TimeSpan(hours:0, minutes:48, seconds:0),
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 2,
                    AppointmentId = 2,
                    ProcedureId = 2,
                    PetId = 8,
                    EmployeeId = 1
                },
                new OrderProcedure
                {
                    Id = 3,
                    Count = 1,
                    Time = new TimeSpan(hours:0, minutes:20, seconds:0),
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 3,
                    AppointmentId = 3,
                    ProcedureId = 9,
                    PetId = 7,
                    EmployeeId = 5
                },
                new OrderProcedure
                {
                    Id = 4,
                    Count = 1,
                    Time = new TimeSpan(hours:0, minutes:55, seconds:0),
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 4,
                    AppointmentId = 4,
                    ProcedureId = 10,
                    PetId = 7,
                    EmployeeId = 4
                },
                new OrderProcedure
                {
                    Id = 5,
                    Count = 1,
                    Time = new TimeSpan(hours:1, minutes:20, seconds:0),
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 5,
                    AppointmentId = 5,
                    ProcedureId = 1,
                    PetId = 1,
                    EmployeeId = 10
                },
                new OrderProcedure
                {
                    Id = 6,
                    Count = 1,
                    Time = new TimeSpan(hours:1, minutes:23, seconds:0),
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 6,
                    AppointmentId = 6,
                    ProcedureId = 2,
                    PetId = 4,
                    EmployeeId = 3
                },
                new OrderProcedure
                {
                    Id = 7,
                    Count = 1,
                    Time = new TimeSpan(hours:1, minutes:1, seconds:0),
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 7,
                    AppointmentId = 7,
                    ProcedureId = 5,
                    PetId = 5,
                    EmployeeId = 4
                },
                new OrderProcedure
                {
                    Id = 8,
                    Count = 1,
                    Time = new TimeSpan(hours:1, minutes:32, seconds:0),
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 8,
                    AppointmentId = 8,
                    ProcedureId = 2,
                    PetId = 10,
                    EmployeeId = 1
                },
                new OrderProcedure
                {
                    Id = 9,
                    Count = 1,
                    Time = new TimeSpan(hours:0, minutes:10, seconds:0),
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 9,
                    AppointmentId = 9,
                    ProcedureId = 2,
                    PetId = 7,
                    EmployeeId = 7
                },
                new OrderProcedure
                {
                    Id = 10,
                    Count = 1,
                    Time = new TimeSpan(hours:1, minutes:11, seconds:0),
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 10,
                    AppointmentId = 10,
                    ProcedureId = 6,
                    PetId = 5,
                    EmployeeId = 8
                },
            };
    }
}
