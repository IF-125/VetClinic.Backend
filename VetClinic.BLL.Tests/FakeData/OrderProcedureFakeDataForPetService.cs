using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;
using VetClinic.Core.Entities.Enums;

namespace VetClinic.BLL.Tests.FakeData
{
    public static class OrderProcedureFakeDataForPetService
    {
        public static List<OrderProcedure> GetOrderProcedureFakeData() =>
        new List<OrderProcedure>
        {
                new OrderProcedure
                {
                    Id = 1,
                    Status= OrderProcedureStatus.Assigned,
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 1,
                    ProcedureId = 4,
                    PetId = 3,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 2,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 2,
                    ProcedureId = 2,
                    PetId = 8,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 3,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 3,
                    ProcedureId = 9,
                    PetId = 7,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 4,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 4,
                    ProcedureId = 10,
                    PetId = 7,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 5,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 5,
                    ProcedureId = 1,
                    PetId = 1,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 6,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 6,
                    ProcedureId = 2,
                    PetId = 4,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 7,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 7,
                    ProcedureId = 5,
                    PetId = 5,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 8,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 8,
                    ProcedureId = 2,
                    PetId = 10,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 9,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 9,
                    ProcedureId = 2,
                    PetId = 7,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
                new OrderProcedure
                {
                    Id = 10,
                    Status= OrderProcedureStatus.NotAssigned,
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 10,
                    ProcedureId = 6,
                    PetId = 5,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe"
                },
        };
    }
}

