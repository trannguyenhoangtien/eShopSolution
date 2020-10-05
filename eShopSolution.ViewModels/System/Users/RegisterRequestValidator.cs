using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required").MaximumLength(200);
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").MaximumLength(200)
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email format not match");
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required").MaximumLength(200);

            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password is at least 6 characters");

            RuleFor(x => x).Custom((request, context) =>
            {
                if(request.Password != request.ConfirmPassword)
                {
                    context.AddFailure("Confirm password is not match");
                }
            });
        }
    }
}
