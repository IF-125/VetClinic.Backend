using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class PetImageValidator:AbstractValidator<PetImage>
    {
        public PetImageValidator()
        {
            RuleFor(x => x.PetId).NotEmpty().WithMessage("PetId should be scpecified");
        }
    }
}
