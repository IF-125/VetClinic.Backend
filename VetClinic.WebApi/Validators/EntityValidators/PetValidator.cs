using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class PetValidator:AbstractValidator <Pet>
    {
        public PetValidator()
        {
            RuleFor(x => x.Breed).NotEmpty().WithMessage("Breed should be added");
        }
    }
}
