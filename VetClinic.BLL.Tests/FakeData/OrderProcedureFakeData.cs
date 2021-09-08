using System.Collections.Generic;
using VetClinic.Core.Entities;
using VetClinic.Core.Entities.Enums;

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
                    Conclusion = "Procedure was conducted successfully.",
                    Details = "The patient appearts to be stable.",
                    OrderId = 1,
                    ProcedureId = 4,
                    Pet = new Pet{ Id=1},
                    PetId=1,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                    Status= OrderProcedureStatus.Assigned
                },
                new OrderProcedure
                {
                    Id = 2,
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 2,
                    ProcedureId = 2,
                    Pet = new Pet{ Id=2},
                    PetId=2,
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                    Status= OrderProcedureStatus.NotAssigned
                },
                new OrderProcedure
                {
                    Id = 3,
                    Conclusion = "Procedure was unsuccessful.",
                    Details = "The patient is in critical condition.",
                    OrderId = 3,
                    Pet = new Pet{ Id=3},
                    PetId=3,
                    ProcedureId = 9,                    
                    EmployeeId = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                    Status= OrderProcedureStatus.Assigned
                },
                new OrderProcedure
                {
                    Id = 4,
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
