using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TempateMicroservice.API.Models;

namespace TempateMicroservice.API.Infrastructure.Validators
{
    public class TemplateAPIValidator: AbstractValidator<TemplateAPI>
    {
        public TemplateAPIValidator()
        {
            RuleFor(item => item.Name)
               .NotEmpty()
               .WithMessage("Template name is empty");
        }
    }
}
