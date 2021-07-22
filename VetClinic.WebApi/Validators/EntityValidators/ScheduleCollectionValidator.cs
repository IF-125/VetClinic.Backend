using FluentValidation;
using System.Collections.Generic;
using VetClinic.Core.Entities;

namespace VetClinic.WebApi.Validators.EntityValidators
{
    public class ScheduleCollectionValidator : AbstractValidator<IEnumerable<Schedule>>
    {
        public ScheduleCollectionValidator()
        {
            RuleForEach(x => x).SetValidator(new ScheduleValidator());
        }
    }
}
