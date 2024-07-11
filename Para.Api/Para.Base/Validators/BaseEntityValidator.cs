using FluentValidation;
using Para.Base.Entity;

namespace Para.Base.Validators;

public class BaseEntityValidator : AbstractValidator<BaseEntity>
{
    public BaseEntityValidator()
    {
        RuleFor(Entity => Entity.InsertDate).NotNull()
            .WithMessage("Insert date is required.");
        RuleFor(Entity => Entity.IsActive).NotNull()
            .WithMessage("IsActive property is required.");
        RuleFor(Entity => Entity.InsertUser).NotNull()
            .WithMessage("Insert User property is required.")
            .MaximumLength(50)
            .WithMessage("Insert User property cannot be greater than 50 characters.");
    }
}