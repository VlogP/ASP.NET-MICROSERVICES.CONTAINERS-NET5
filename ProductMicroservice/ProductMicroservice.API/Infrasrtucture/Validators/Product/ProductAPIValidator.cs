using FluentValidation;
using ProductMicroservice.API.Models.Product;

namespace ProductMicroservice.API.Infrasrtucture.Validators.Product
{
    public class ProductAPIValidator : AbstractValidator<ProductPostAPI>
    {
        public ProductAPIValidator()
        {
            RuleFor(item => item.Name)
               .NotEmpty()
               .WithMessage("Product name is empty");

            RuleFor(item => item.ClientId)
               .NotEmpty()
               .WithMessage("Client id is empty")
               .Matches(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$")
               .WithMessage("Wrong Client id format");

            RuleFor(item => item.Description)
               .NotEmpty()
               .WithMessage("Description is empty")
               .MaximumLength(500)
               .WithMessage("Maximum length is 500");
        }
    }
}
