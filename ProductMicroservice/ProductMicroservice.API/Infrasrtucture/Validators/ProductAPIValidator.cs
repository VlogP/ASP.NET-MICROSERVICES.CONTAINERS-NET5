using FluentValidation;
using ProductMicroservice.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.API.Infrasrtucture.Validators
{
    public class ProductAPIValidator : AbstractValidator<ProductAPI>
    {
        public ProductAPIValidator()
        {
            RuleFor(item => item.Name)
               .NotEmpty()
               .WithMessage("Product name is empty");
        }
    }
}
