using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.WorkItems.Commands.DeleteWorkItem
{
    public class DeleteWorkItemCommand : IRequest
    {
        public int Id { get; set; }
    }
    public class DeleteWorkItemCommandHandler : IRequestHandler<DeleteWorkItemCommand, Unit>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public DeleteWorkItemCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<Unit> Handle(DeleteWorkItemCommand request, CancellationToken cancellationToken)
        {
            var toRemove = _applicationDbContext.WorkItems
                .Where(workItem => workItem.Id == request.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(WorkItem), request.Id);
            _applicationDbContext.WorkItems.Remove(toRemove);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
