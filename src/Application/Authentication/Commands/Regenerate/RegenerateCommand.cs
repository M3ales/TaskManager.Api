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
using TaskManager.Api.Domain.Entities.Auth;

namespace TaskManager.Api.Application.Authentication.Commands.Regenerate
{
    [Authorise]
    public class RegenerateCommand : IRequest<string>
    {
        public int AccountId { get; set; }
    }
    public class RegenerateCommandHandler : IRequestHandler<RegenerateCommand, string>
    {
        private readonly IApplicationDbContext _applicationDbContext;
        public RegenerateCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<string> Handle(RegenerateCommand request, CancellationToken cancellationToken)
        {
            var account = _applicationDbContext.Accounts
                .Where(account => account.Id == request.AccountId)
                .FirstOrDefault() ?? throw new NotFoundException(nameof(Account), request.AccountId);

            account.RefreshToken = Guid.NewGuid().ToString();
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
            return account.RefreshToken;
        }
    }
}
