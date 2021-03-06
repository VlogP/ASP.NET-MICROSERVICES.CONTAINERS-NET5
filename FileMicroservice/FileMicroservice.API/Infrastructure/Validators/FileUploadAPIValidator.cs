using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileMicroservice.API.Models;

namespace FileMicroservice.API.Infrastructure.Validators
{
    public class FileUploadAPIValidator: AbstractValidator<FileUploadAPI>
    {
        public FileUploadAPIValidator()
        {
            RuleFor(item => item.File)
               .NotEmpty()
               .WithMessage("File is not selected");

            RuleFor(item => item.ReportId)
               .NotEmpty()
               .WithMessage("Report is not selected");
        }
    }
}
