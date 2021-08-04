using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Attributes;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities;

namespace TaskManager.Api.Application.ProgressItems.Commands.UpdateProgressItem
{
    [Authorise]
    public class UpdateProgressItemCommand : IRequest
    {
        public int Id { get; set; }
        public bool Complete { get; set; }
    }
    public class UpdateProgressItemCommandHandler : IRequestHandler<UpdateProgressItemCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        public UpdateProgressItemCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateProgressItemCommand request, CancellationToken cancellationToken)
        {
            var progressItem =_applicationDbContext.ProgressItems
                .Where(progressItem => progressItem.Id == request.Id)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(ProgressItem), request.Id);
            progressItem.Complete = request.Complete;
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
