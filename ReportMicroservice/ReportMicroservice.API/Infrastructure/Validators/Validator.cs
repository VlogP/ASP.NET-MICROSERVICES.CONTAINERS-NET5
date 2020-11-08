using FluentValidation;
using ReportMicroservice.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportMicroservice.API.Infrastructure.Validators
{
    public class ReportAPIValidator : AbstractValidator<ReportAPI>
    {
        public ReportAPIValidator()
        {
            RuleFor(item => item.Name)
               .NotEmpty()
               .WithMessage("Report name is empty");
        }
    }
}
