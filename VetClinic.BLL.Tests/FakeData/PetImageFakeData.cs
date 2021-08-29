using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.BLL.Tests.FakeData
{
    public static class PetImageFakeData
    {
        public static List<PetImage> GetImageFakeData()
        {
            return new List<PetImage>
            {
                new PetImage
                {
                    Id=1,
                    PetId=1
                },
                new PetImage
                {
                    Id=2,
                    PetId=1
                },
                new PetImage
                {
                    Id=3,
                    PetId=3
                },
                new PetImage
                {
                    Id=4,
                    PetId=4
                },
                new PetImage
                {
                    Id=5,
                    PetId=5
                },
                new PetImage
                {
                    Id=6,
                    PetId=6
                },
                new PetImage
                {
                    Id=7,
                    PetId=7
                },
                new PetImage
                {
                    Id=8,
                    PetId=8
                },
                new PetImage
                {
                    Id=9,
                    PetId=9
                },
                new PetImage
                {
                    Id=10,
                    PetId=10
                }
            };
        }
    }
}
