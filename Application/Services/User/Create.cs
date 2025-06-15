using Application.SeedWork.Responses;
using FluentValidation;
using Infrastructure.Extensions;
using Infrastructure.UnitOfWork;
using Mapster;
using MediatR;

namespace Application.Services.User;

public class Create
{
    public class Request : IRequest<BaseResponse<Response>>
    {
        public Request(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }

        public string Username { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
    }
    
    internal class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(request => request.Username)
                .NotEmpty().WithMessage("Username is required");
        
            RuleFor(request => request.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is invalid");
        
            RuleFor(request => request.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one upper case letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lower case letter")
                .Matches("\\d").WithMessage("Password must contain at least one number digit")
                .Matches("[^\\w\\d ]").WithMessage("Password must contain at least one special character");
        }
    }
    
    public class Response
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
    }
    
    internal class Service : IRequestHandler<Request, BaseResponse<Response>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<Request> _validator;

        public Service(IUnitOfWork uow, IValidator<Request> validator)
        {
            _uow = uow;
            _validator = validator;
        }

        public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken ct)
        {
            var validationResult = await _validator.ValidateAsync(request);
            
            if (!validationResult.IsValid)
                return new ErrorListResponse<Response>(422, "One or more inputs are incorrect", validationResult.Errors);
            
            var user = request.Adapt<Core.User>();

            user.SetPassword(user.Password.HashPassword());

            await _uow.UserRepository.AddAsync(user, ct);
            await _uow.CommitAsync(ct);

            var data = user.Adapt<Response>();

            return new DataResponse<Response>(201, "User created", data);
        }
    }
}