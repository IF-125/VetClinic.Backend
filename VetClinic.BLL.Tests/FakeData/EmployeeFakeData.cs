using System.Collections.Generic;
using VetClinic.Core.Entities;

namespace VetClinic.BLL.Tests.FakeData
{
    public static class EmployeeFakeData
    {
        public static List<Employee> GetEmployeeFakeData() =>
            new List<Employee>
            {
                new Employee
                {
                    Id = "f1a05cca-b479-4f72-bbda-96b8979f4afe",
                    FirstName = "Bob",
                    LastName = "Roberts",
                    Address = "9 Grayhawk Alley",
                    Email = "tisacq0@unesco.org"
                },
                new Employee
                {
                    Id = "6fca381a-40d0-4bf9-a076-706e1a995662",
                    FirstName = "Roselia",
                    LastName = "Isacq",
                    Address = "3 Menomonie Point",
                    Email = "dbrandle2@arizona.edu"
                },
                new Employee
                {
                    Id = "804bbbca-3ffc-4d28-9b71-0d7788ddf681",
                    FirstName = "Bob",
                    LastName = "Roberts",
                    Address = "9 Grayhawk Alley",
                    Email = "tisacq0@unesco.org"
                },
                new Employee
                {
                    Id = "0e216f00-f03b-4655-9f06-f166828d35df",
                    FirstName = "Mill",
                    LastName = "Hixson",
                    Address = "9170 Arapahoe Junction",
                    Email = "mhixson3@tmall.com"
                },
                new Employee
                {
                    Id = "ada82298-b807-4267-a9a2-c3955e975294",
                    FirstName = "Bryna",
                    LastName = "McTrustey",
                    Address = "2 Roth Point",
                    Email = "bmctrustey4@weather.com"
                },
                new Employee
                {
                    Id = "70efa05d-a06d-45b2-b116-af1ccb602d2e",
                    FirstName = "Erek",
                    LastName = "Dosdale",
                    Address = "5357 Schmedeman Drive",
                    Email = "edosdalef@google.nl"
                },
                new Employee
                {
                    Id = "7a7dc85b-ee14-4643-9782-4e4e98855d41",
                    FirstName = "Philbert",
                    LastName = "Gauthorpp",
                    Address = "55 Cottonwood Circle",
                    Email = "rgladebeck7@myspace.com"
                },
                new Employee
                {
                    Id = "982ed974-1d5e-449d-a94c-09a68f01f7e4",
                    FirstName = "Noel",
                    LastName = "Pont",
                    Address = "9 Grayhawk Alley",
                    Email = "npontm@upenn.edu"
                },
                new Employee
                {
                    Id = "b8e7017a-bec3-4654-9c40-538758b55917",
                    FirstName = "Martin",
                    LastName = "Roberts",
                    Address = "9 Grayhawk Alley",
                    Email = "smereweathern@elpais.com"
                },
                new Employee
                {
                    Id = "b8e7017a-bec3-4654-9c40-538758b55917",
                    FirstName = "Derek",
                    LastName = "Hussy",
                    Address = "1 Katie Court",
                    Email = "khussyo@elegantthemes.com"
                }
            };
    }
}
