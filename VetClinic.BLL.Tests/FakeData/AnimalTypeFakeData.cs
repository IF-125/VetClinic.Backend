using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.BLL.Tests.FakeData
{
    public static class AnimalTypeFakeData
    {
        public static List<AnimalType> GetAnimalTypeFakeData() =>
            new List<AnimalType>
            {
                new AnimalType
                {
                    Id=1,
                    Type="Dog1"                   
                },
                new AnimalType
                {
                    Id=2,
                    Type="Dog2"
                },
                 new AnimalType
                {
                    Id=3,
                    Type="Dog3"
                },
                new AnimalType
                {
                    Id=4,
                    Type="Dog4"
                }, new AnimalType
                {
                    Id=5,
                    Type="Dog5"
                },
                new AnimalType
                {
                    Id=6,
                    Type="Dog6"
                }, new AnimalType
                {
                    Id=7,
                    Type="Dog7"
                },
                new AnimalType
                {
                    Id=8,
                    Type="Dog8"
                },
                 new AnimalType
                {
                    Id=9,
                    Type="Dog9"
                },
                new AnimalType
                {
                    Id=10,
                    Type="Dog10"
                }
            };
    }
}
