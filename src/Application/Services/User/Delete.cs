using Application.SeedWork.Responses;
using Infrastructure.SeedWork.UnitOfWork;
using MediatR;

namespace Application.Services.User;

public class Delete
{
    public class Request(Guid id) : IRequest<BaseResponse<object>>
    {
        public Guid Id { get; private set; } = id;
    }

    internal class Service : IRequestHandler<Request, BaseResponse<object>>
    {
        private readonly IUnitOfWork _uow;

        public Service(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<BaseResponse<object>> Handle(Request request, CancellationToken ct)
        {
            var user = await _uow.UserRepository.GetByIdAsync(request.Id, ct);
            
            if (user == null)
                return new NoDataResponse<object>(404, "User does not exist");

            await _uow.UserRepository.DeleteAsync(request.Id, ct);
            await _uow.CommitAsync(ct);
            
            return new NoDataResponse<object>("User deleted");
        }
    }
}