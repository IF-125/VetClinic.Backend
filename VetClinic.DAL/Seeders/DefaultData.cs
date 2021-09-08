using System;
using System.Linq;
using VetClinic.Core.Entities;
using VetClinic.Core.Entities.Enums;
using VetClinic.DAL.Context;

namespace VetClinic.DAL.Seeders
{
    class DefaultData
    {
        public static void Initialize(VetClinicDbContext context)
        {
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }
            context.Orders.AddRange(
                    new Order
                    {
                        Id = 1,
                        CreatedAt = DateTime.Today,
                        PaymentOption = PaymentOption.Cash,

                    },
                    new Order
                    {
                        Id = 2,
                        CreatedAt = new DateTime(2021, 08, 05, 20, 08, 37, 4988622),
                        PaymentOption = PaymentOption.Cash,
                    },
                    new Order
                    {
                        Id = 3,
                        CreatedAt = DateTime.Now,
                        PaymentOption = PaymentOption.Card
                    },
                    new Order
                    {
                        Id = 4,
                        CreatedAt = DateTime.Now,
                        PaymentOption = PaymentOption.Card,
                    },
                    new Order
                    {
                        Id = 5,
                        CreatedAt = DateTime.Now,
                        PaymentOption = PaymentOption.Cash,
                    }
                ); ;
            context.SaveChanges();
            context.Users.AddRange(
                    new User
                    {
                        FirstName = "Adam",
                        LastName = "Harrison",
                        UserName = "adam@gmail.com",
                        Email = "adam@gmail.com"
                    },
                    new User
                    {
                        FirstName = "Jimmy",
                        LastName = "McGill",
                        UserName = "chuck@gmail.com",
                        Email = "chuck@gmail.com",
                    }
                );
            context.SaveChanges();
            context.Procedures.AddRange(
                    new Procedure
                    {
                        Id =1,
                        Title = "Vaccination",
                        Description = "Regular vaccination",
                        Duration = TimeSpan.FromMinutes(10),
                        Price = 400.00m,
                        AnimalTypesProcedures = 
                        { 

                        },
                        OrderProcedures =
                        {

                        }
                    },
                    new Procedure
                    {
                        Id = 3,
                        Title = "Esophagoscopy",
                        Description = "Medical procedure that allows a doctor to look inside esophagus",
                        Duration = TimeSpan.FromMinutes(20),
                        Price = 600.00m,
                        AnimalTypesProcedures =
                        {

                        },
                        OrderProcedures =
                        {

                        }
                    },
                     new Procedure
                     {
                         Id = 3,
                         Title = "Vasectomy",
                         Description = "A form of male birth control that cuts the supply of sperm to your semen",
                         Duration = TimeSpan.FromMinutes(20),
                         Price = 600.00m,
                         AnimalTypesProcedures =
                        {

                        },
                         OrderProcedures =
                        {
                             
                        }
                     }
                );
            context.SaveChanges();
            context.OrderProcedures.AddRange(
                    new OrderProcedure
                    {
                        Id = 1,
                        Conclusion = "Everything went well",
                        Details = "No details",
                        OrderId = 1,
                        ProcedureId = 2,
                        PetId = 1,
                        EmployeeId = "cbea1ab5-4841-4e52-8883-9829c8007c9a",
                        Status = OrderProcedureStatus.Completed
                    },
                    new OrderProcedure
                    {
                        Id = 2,
                        Conclusion = null,
                        Details = null,
                        OrderId = 2,
                        ProcedureId = 3,
                        PetId = 2,
                        EmployeeId = "38bc1b9e-d487-4f38-80cb-808ea0d72e08",
                        Status = OrderProcedureStatus.NotAssigned
                    },
                    new OrderProcedure
                    {
                        Id = 3,
                        Conclusion = null,
                        Details = null,
                        OrderId = 3,
                        ProcedureId = 3,
                        PetId = 1,
                        EmployeeId = "38bc1b9e-d487-4f38-80cb-808ea0d72e08",
                        Status = OrderProcedureStatus.NotAssigned
                    },
                    new OrderProcedure
                    {
                        Id = 4,
                        Conclusion = null,
                        Details = null,
                        OrderId = 4,
                        ProcedureId = 3,
                        PetId = 2,
                        EmployeeId = null,
                        Status = OrderProcedureStatus.NotAssigned
                    },
                    new OrderProcedure
                    {
                        Id = 5,
                        Conclusion = null,
                        Details = null,
                        OrderId = 5,
                        ProcedureId = 3,
                        PetId = 1,
                        EmployeeId = null,
                        Status = OrderProcedureStatus.NotAssigned
                    }
                );
            context.SaveChanges();
            context.Appointments.AddRange(
                    new Appointment
                    {
                        Id = 1 ,
                        Status = AppointmentStatus.Opened,
                        From = DateTime.Now,
                        To = DateTime.Now.AddMinutes(15),
                        OrderProcedureId = null
                    },
                    new Appointment
                    {
                        Id = 2,
                        Status = AppointmentStatus.Opened,
                        From = DateTime.Now,
                        To = DateTime.Now.AddMinutes(30),
                        OrderProcedureId = null
                    },
                    new Appointment
                    {
                        Id = 3,
                        Status = AppointmentStatus.Opened,
                        From = DateTime.Now,
                        To = DateTime.Now.AddMinutes(30),
                        OrderProcedureId = null

                    },
                    new Appointment
                    {
                        Id = 4,
                        Status = 0,
                        From = DateTime.Now,
                        To = DateTime.Now.AddMinutes(40),
                        OrderProcedureId = 2
                    },
                    new Appointment
                    {
                        Id = 5,
                        Status = 0,
                        From = DateTime.Now,
                        To = DateTime.Now.AddMinutes(30),
                        OrderProcedureId = 3
                    });
            context.SaveChanges();
            context.AnimalTypes.AddRange(
                new AnimalType
                {
                    Id = 1,
                    Type = "Cat"
                },
                new AnimalType 
                { 
                    Id = 2,
                    Type = "Dog"
                },
                new AnimalType
                {
                    Id = 3,
                    Type = "Parrot"
                },
                new AnimalType
                {
                    Id = 4,
                    Type = "Fish"
                },
                new AnimalType
                {
                    Id = 5,
                    Type = "Lizard"
                });
            context.SaveChanges();
            context.AnimalTypeProcedures.AddRange(
                new AnimalTypeProcedure
                {
                    AnimalTypeId =1,
                    ProcedureId =3,
                },
                new AnimalTypeProcedure
                {
                     AnimalTypeId = 2,
                     ProcedureId = 3,
                });
            context.Pets.AddRange(
                new Pet
                {
                    Id = 1,
                    Name = "Murka",
                    Information = "Very peaceful cat",
                    Breed = "Some breed",
                    Age = 4,
                    ClientId = "3aa830e0 - 99d6 - 4c04 - 839c - 8a26ee5acbd3",
                    AnimalTypeId = 1
                },
                new Pet
                {
                    Id = 2,
                    Name = "Jack",
                    Information = "Very peaceful dog",
                    Breed = "Some breed",
                    Age = 6,
                    ClientId = "3aa830e0 - 99d6 - 4c04 - 839c - 8a26ee5acbd3",
                    AnimalTypeId = 2
                });

            context.PhoneNumbers.AddRange(
                new PhoneNumber
                {
                    Id = 1,
                    Phone = "123123123",
                    ClientId = "3aa830e0-99d6-4c04-839c-8a26ee5acbd3"
                },
                new PhoneNumber
                {
                    Id = 2,
                    Phone = "123123434",
                    ClientId = "3aa830e0-99d6-4c04-839c-8a26ee5acbd3"
                });
            context.Positions.AddRange(
                new Position
                {
                    Id = 1,
                    Title = "Doctor"
                },
                new Position
                {
                    Id = 2,
                    Title = "Accounter"
                });
            context.EmployeePositions.AddRange(
                new EmployeePosition
                {
                    Id = 1,
                    CurrentBaseSalary = 1000m,
                    Rate = 20,
                    HierdDate = DateTime.Now,
                    DismissedDate = null,
                    EmployeeId = "38bc1b9e-d487-4f38-80cb-808ea0d72e08",
                    PositionId = 1
                },
                new EmployeePosition
                {
                    Id = 2,
                    CurrentBaseSalary = 1000m,
                    Rate = 20,
                    HierdDate = DateTime.Now,
                    DismissedDate = null,
                    EmployeeId = "cbea1ab5-4841-4e52-8883-9829c8007c9a",
                    PositionId = 1
                });
            context.SaveChanges();
            }
        }
    }

