using FluentValidation;
using Para.Data.Domain;

namespace Para.Data.Validators;

public class CustomerPhoneValidator : AbstractValidator<CustomerPhone>
{
    public CustomerPhoneValidator()
    {
        RuleFor(customerPhone => customerPhone.CustomerId).NotNull()
            .WithMessage("Id is required.");
        RuleFor(customerPhone => customerPhone.CountyCode).NotNull()
            .WithMessage("Country code is required.")
            .MaximumLength(3)
            .WithMessage("Country code cannot be greater than 3 characters.");
        RuleFor(customerPhone => customerPhone.Phone).NotNull()
            .WithMessage("Phone number is required.")
            .MaximumLength(10)
            .WithMessage("Phone number cannot be greater than 10 characters.");
        RuleFor(customerPhone => customerPhone.IsDefault).NotNull()
            .WithMessage("Customer phone number is required.");
    }
}