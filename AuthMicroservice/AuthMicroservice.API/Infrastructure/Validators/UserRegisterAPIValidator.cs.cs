using AuthMicroservice.API.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthMicroservice.API.Infrastructure.Validators
{
    public class UserRegisterAPIValidator: AbstractValidator<UserRegisterAPI>
    {
        public UserRegisterAPIValidator()
        {
            RuleFor(item => item.Email)
                .NotNull()
                .WithMessage("Email is empty")
                .EmailAddress()
                .WithMessage("Email has wrong format");

            RuleFor(item => item.Password)
                .NotNull()
                .WithMessage("Password is empty")
                .Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
                .WithMessage("Password must have next format - at least one upper case english letter, at least one lower case english letter, at least one digit, at least one special character, minimum 8 in length, maximum 50 in length");

        }
    }
}
