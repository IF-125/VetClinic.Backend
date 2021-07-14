using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.BLL.Tests.FakeData
{
    public static class PetFakeData
    {
        public static List<Pet> GetPetFakeData() =>
            new List<Pet>
            {
                new Pet
                {
                    Id = 1,
                    Name = "Lord1",
                    Information = "Animal from the street1",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 2,
                    Name = "Lord2",
                    Information = "Animal from the street2",
                    Breed = "Persian",
                    Age = 2
                },
                new Pet
                {
                    Id = 3,
                    Name = "Lord3",
                    Information = "Animal from the street3",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 4,
                    Name = "Lord4",
                    Information = "Animal from the street4",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 5,
                    Name = "Lord5",
                    Information = "Animal from the street5",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 6,
                    Name = "Lord6",
                    Information = "Animal from the street6",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 7,
                    Name = "Lord7",
                    Information = "Animal from the street7",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 8,
                    Name = "Lord8",
                    Information = "Animal from the street8",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 9,
                    Name = "Lord9",
                    Information = "Animal from the street9",
                    Breed = "Bebgal",
                    Age = 2
                },
                new Pet
                {
                    Id = 10,
                    Name = "Lord10",
                    Information = "Animal from the street10",
                    Breed = "Bebgal",
                    Age = 2
                }
            };
    }
}
