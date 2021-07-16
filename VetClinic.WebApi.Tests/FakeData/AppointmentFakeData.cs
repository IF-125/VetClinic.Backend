using System;
using System.Collections.Generic;
using VetClinic.Core.Entities;
namespace VetClinic.WebApi.Tests.FakeData
{
    public static class AppointmentFakeData
    {
        public static List<Appointment> GetAppointmentFakeData() =>
            new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    Status = AppointmentStatus.Opened,
                    From = new DateTime(2021, 7, 10),
                    To = new DateTime(2021, 7, 11),
                    OrderProcedureId = 1
                },
                new Appointment
                {
                    Id = 2,
                    Status = AppointmentStatus.Closed,
                    From = new DateTime(2021, 7, 12),
                    To = new DateTime(2021, 7, 13),
                    OrderProcedureId = 2
                },
                new Appointment
                {
                    Id = 3,
                    Status = AppointmentStatus.Closed,
                    From = new DateTime(2021, 7, 13),
                    To = new DateTime(2021, 7, 14),
                    OrderProcedureId = 3
                },
                new Appointment
                {
                    Id = 4,
                    Status = AppointmentStatus.Closed,
                    From = new DateTime(2021, 7, 13),
                    To = new DateTime(2021, 7, 14),
                    OrderProcedureId = 4
                },
                new Appointment
                {
                    Id = 5,
                    Status = AppointmentStatus.Opened,
                    From = new DateTime(2021, 7, 22),
                    To = new DateTime(2021, 7, 23),
                    OrderProcedureId = 5
                },
                new Appointment
                {
                    Id = 6,
                    Status = AppointmentStatus.Opened,
                    From = new DateTime(2021, 7, 23),
                    To = new DateTime(2021, 7, 24),
                    OrderProcedureId = 6
                },
                new Appointment
                {
                    Id = 7,
                    Status = AppointmentStatus.Opened,
                    From = new DateTime(2021, 7, 30),
                    To = new DateTime(2021, 7, 31),
                    OrderProcedureId = 7
                },
                new Appointment
                {
                    Id = 8,
                    Status = AppointmentStatus.Opened,
                    From = new DateTime(2021, 8, 11),
                    To = new DateTime(2021, 8, 12),
                    OrderProcedureId = 8
                },
                new Appointment
                {
                    Id = 9,
                    Status = AppointmentStatus.Opened,
                    From = new DateTime(2021, 7, 14),
                    To = new DateTime(2021, 7, 15),
                    OrderProcedureId = 9
                },
                new Appointment
                {
                    Id = 10,
                    Status = AppointmentStatus.Opened,
                    From = new DateTime(2021, 7, 18),
                    To = new DateTime(2021, 7, 19),
                    OrderProcedureId = 10
                }
            };
    }
}
