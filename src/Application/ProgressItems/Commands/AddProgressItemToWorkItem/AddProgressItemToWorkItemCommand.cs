using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.ProgressItems.Commands.AddProgressItemToWorkItem
{

    public class AddProgressItemToWorkItemCommand : IRequest<int>
    {
        public int WorkItemId { get; set; }
        public string Name { get; set; }
        public bool Complete { get; set; }
    }

    public class AddProgressItemToWorkItemCommandHandler : IRequestHandler<AddProgressItemToWorkItemCommand, int>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public AddProgressItemToWorkItemCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<int> Handle(AddProgressItemToWorkItemCommand request, CancellationToken cancellationToken)
        {
            var workItem = _applicationDbContext.WorkItems
                .Where(workItem => workItem.Id == request.WorkItemId)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(WorkItem), request.WorkItemId);
            var progressItem = new ProgressItem()
            {
                Name = request.Name,
                Complete = request.Complete,
                WorkItem = workItem
            };
            _applicationDbContext.ProgressItems.Add(progressItem);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return progressItem.Id;
        }
    }
}
