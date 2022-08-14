using Common.Architecture.Core.Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Architecture.Infrastructure.ValidationRules.FluentValidation
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(p => p.UserName).MinimumLength(10);
            RuleFor(p => p.Surname).NotEmpty();
            //RuleFor(p => p.Price).GreaterThan(0);
            //RuleFor(p => p.Price).GreaterThanOrEqualTo(1000).When(p => p.BrandId == 1);

            // Plaka Numaralarım No ile başlamalı.
            //RuleFor(p => p.PlateNo).Must(StartWithNumber).WithMessage("Araçların plaka numaraları sayı ile başlamalıdır!");
        }

        private bool StartWithNumber(string arg)
        {
            if (arg.StartsWith("0") || arg.StartsWith("1") ||
                arg.StartsWith("2") || arg.StartsWith("3") ||
                arg.StartsWith("4") || arg.StartsWith("5") ||
                arg.StartsWith("6") || arg.StartsWith("7") ||
                arg.StartsWith("8") || arg.StartsWith("9"))
                return true;
            return false;
        }
    }
}
