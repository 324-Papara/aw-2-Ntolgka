using FluentValidation;

namespace Para.Api.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.Id).NotNull()
            .WithMessage("ID is required")
            .GreaterThanOrEqualTo(1)
            .WithMessage("ID must be greater than or equal to 1.")
            .LessThanOrEqualTo(10000)
            .WithMessage("ID must be less than or equal to 10000.");
        
        RuleFor(book => book.Name).NotNull()
            .WithMessage("Book name is required.")
            .MaximumLength(50)
            .WithMessage("Book name must have a maximum of 50 characters.")
            .MinimumLength(5)
            .WithMessage("Book name must have a minimum of 5 characters");
        
        RuleFor(book => book.Author).NotNull()
            .WithMessage("Book author is required.")
            .MaximumLength(50)
            .WithMessage("Book author must have a maximum of 50 characters.")
            .MinimumLength(5)
            .WithMessage("Book author must have a minimum of 5 characters");
        
        RuleFor(book => book.PageCount).NotNull()
            .WithMessage("Book page count is required.")
            .GreaterThanOrEqualTo(50)
            .WithMessage("Book page count must be greater than or equal to 50.")
            .LessThanOrEqualTo(400)
            .WithMessage("Book page count must be less than or equal to 400.");
        
        RuleFor(Book => Book.Year).NotNull()
            .WithMessage("Book year is required.")
            .GreaterThanOrEqualTo(1900)
            .WithMessage("Book year must be greater than or equal to 1900.")
            .LessThanOrEqualTo(2024)
            .WithMessage("Book year must be less than or equal to 2024.");
    }
}