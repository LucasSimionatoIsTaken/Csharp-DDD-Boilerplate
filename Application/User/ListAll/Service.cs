using Infrastructure.Contexts;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.UnitOfWork;
using Mapster;
using MapsterMapper;
using MediatR;

namespace Application.User.ListAll;

public class Service : IRequestHandler<Request, List<Response>>
{
    private readonly IUserRepository _repository;
    private readonly IUnitOfWork _uow;

    public Service(IUserRepository repository, IUnitOfWork uow)
    {
        _repository = repository;
        _uow = uow;
    }

    public async Task<List<Response>> Handle(Request request, CancellationToken ct)
    {
        var users = await _uow.UserRepository.ListAllAsync(ct);

        return users.Adapt<List<Response>>();
    }
}