using Application.SeedWork.Responses;
using Infrastructure.SeedWork.UnitOfWork;
using Mapster;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Services.User;

public class ListAll
{
    public class RequestQuery : IRequest<BaseResponse<Response>>
    {
        [SwaggerSchema("Actual page, starts in 1")]
        public int Page { get; set; } = 1;
        
        [SwaggerSchema("Number of items per page")]
        public int PageSize { get; set; } = 20;
    }
    
    public class Response
    {
        public string Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
    }

    internal class Service : IRequestHandler<RequestQuery, BaseResponse<Response>>
    {
        private readonly IUnitOfWork _uow;

        public Service(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<BaseResponse<Response>> Handle(RequestQuery query, CancellationToken ct)
        {
            var (users, total) = await _uow.UserRepository.ListAllAsync(
                page: query.Page,
                pageSize: query.PageSize,
                ct: ct
                );

            var data = users.Adapt<List<Response>>();
            
            return new PaginatedResponse<Response>(data, query.Page, query.PageSize, total);
        }
    }
}