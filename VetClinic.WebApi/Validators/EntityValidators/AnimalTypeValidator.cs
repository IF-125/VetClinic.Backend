using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    class AnimalTypeValidator:AbstractValidator<AnimalType>
    {
        public AnimalTypeValidator()
        {
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type should be added");
        }
    }
}
