using FluentValidation;
using ProductMicroservice.API.Models.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.API.Infrasrtucture.Validators.Client
{
    public class ClientAPIValidator : AbstractValidator<ClientPostAPI>
    {
        public ClientAPIValidator()
        {
            RuleFor(item => item.Name)
               .NotEmpty()
               .WithMessage("Client name is empty");
        }
    }
}
