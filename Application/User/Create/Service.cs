using Infrastructure.Extensions;
using Infrastructure.UnitOfWork;
using Mapster;
using MediatR;

namespace Application.User.Create;

public class Service : IRequestHandler<Request, Response>
{
    private readonly IUnitOfWork _uow;

    public Service(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Response> Handle(Request request, CancellationToken ct)
    {
        var user = request.Adapt<Core.User>();

        user.SetPassword(user.Password.GenerateBCryptHash());

        await _uow.UserRepository.AddAsync(user, ct);
        await _uow.CommitAsync(ct);

        return user.Adapt<Response>();
    }
}