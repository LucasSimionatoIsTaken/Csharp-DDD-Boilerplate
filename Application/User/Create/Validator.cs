using FluentValidation;

namespace Application.User.Create;

public class Validator : AbstractValidator<Request>
{
    public Validator()
    {
        RuleFor(request => request.Username)
            .NotEmpty().WithMessage("Username is required");
        
        RuleFor(request => request.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid");
        //TODO: Custom rule for email already exists
        
        RuleFor(request => request.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one upper case letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lower case letter")
            .Matches("\\d").WithMessage("Password must contain at least one number digit")
            .Matches("[^\\w\\d ]").WithMessage("Password must contain at least one special character");
    }
}