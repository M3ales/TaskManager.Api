using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.ProgressItems.Commands.DeleteProgressItem
{
    public class DeleteProgressItemCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteProgressItemCommandHandler : IRequestHandler<DeleteProgressItemCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public DeleteProgressItemCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(DeleteProgressItemCommand request, CancellationToken cancellationToken)
        {
            var item = _applicationDbContext.ProgressItems
                .Where(item => item.Id == request.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(ProgressItem), request.Id);
            _applicationDbContext.ProgressItems.Remove(item);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
