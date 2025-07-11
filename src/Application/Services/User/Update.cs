using Application.SeedWork.Responses;
using FluentValidation;
using Infrastructure.Extensions;
using Infrastructure.SeedWork.UnitOfWork;
using Mapster;
using MediatR;

namespace Application.Services.User;

public class Update
{
    public class Request : IRequest<BaseResponse<Response>>
    {
        public Guid Id { get; private set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        
        public void SetId(Guid id) => Id = id;
    }

    internal class Validator : AbstractValidator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id cannot be empty");

            RuleFor(request => request.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Email is invalid");

            RuleFor(request => request.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches("[A-Z]").WithMessage("Password must contain at least one upper case letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lower case letter")
                .Matches("\\d").WithMessage("Password must contain at least one number digit")
                .Matches("[^\\w\\d ]").WithMessage("Password must contain at least one special character")
                .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }

    internal class Mapping : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Request, Core.User>()
                .IgnoreNullValues(true);
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
        private readonly TypeAdapterConfig _config;

        public Service(IUnitOfWork uow, IValidator<Request> validator, TypeAdapterConfig config)
        {
            _uow = uow;
            _validator = validator;
            _config = config;
        }

        public async Task<BaseResponse<Response>> Handle(Request request, CancellationToken ct)
        {
            var validationResult = await _validator.ValidateAsync(request);

            if (!validationResult.IsValid)
                return new ErrorListResponse<Response>("One or more inputs are incorrect", validationResult.Errors);

            var oldUser = await _uow.UserRepository.GetByIdAsync(request.Id, ct);
            
            if (oldUser == null)
                return new NoDataResponse<Response>(404, "User does not exist");

            if (request.Password != null)
                request.Password = request.Password.HashPassword();

            request.Adapt(oldUser, _config);
            
            _uow.UserRepository.Update(oldUser, ct);
            await _uow.CommitAsync(ct);
            
            var res = oldUser.Adapt<Response>();
            
            return new DataResponse<Response>("User updated", res);
        }
    }
}