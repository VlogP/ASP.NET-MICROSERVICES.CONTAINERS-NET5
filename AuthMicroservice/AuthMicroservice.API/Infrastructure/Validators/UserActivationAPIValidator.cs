using AuthMicroservice.API.Models.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Infrastructure.Validators
{
    public class UserActivationAPIValidator : AbstractValidator<UserActivationAPI>
    {
        public UserActivationAPIValidator()
        {
            RuleFor(item => item.ActivationEmail)
               .NotEmpty()
               .WithMessage("Email is empty")
               .EmailAddress()
               .WithMessage("Email has wrong format");

            RuleFor(item => item.ActivationCode)
               .NotEmpty()
               .WithMessage("Activation code is empty");
        }
    }
}
