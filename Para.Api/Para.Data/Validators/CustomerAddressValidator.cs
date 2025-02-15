using FluentValidation;
using Para.Data.Domain;

namespace Para.Data.Validators;

public class CustomerAddressValidator: AbstractValidator<CustomerAddress>
{
    public CustomerAddressValidator()
    {
        RuleFor(address => address.CustomerId).NotNull()
            .WithMessage("Customer Id is required.");
        RuleFor(address => address.Country).NotNull()
            .WithMessage("Country is required.")
            .MaximumLength(50)
            .WithMessage("Country cannot have more than 50 characters.");
        RuleFor(address => address.City).NotNull()
            .WithMessage("City is required.")
            .MaximumLength(50)
            .WithMessage("City cannot have more than 50 characters.");
        RuleFor(address => address.AddressLine).NotNull()
            .WithMessage("Address line is required.")
            .MaximumLength(250)
            .WithMessage("Country cannot have more than 250 characters.");
        RuleFor(address => address.ZipCode)
            .MaximumLength(6)
            .WithMessage("zip code cannot have more than 6 characters.");
        RuleFor(address => address.IsDefault).NotNull()
            .WithMessage("IsDefault property cannot be null.");
    }
}