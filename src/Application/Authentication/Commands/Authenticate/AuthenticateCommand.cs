using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;

namespace TaskManager.Api.Application.Authentication.Commands.Authenticate
{
    public class AuthenticateCommand : IRequest<string>
    {
        public string RefreshToken { get; set; }
    }
    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, string>
    {
        private readonly IAuthService _authenticationService;
        public AuthenticateCommandHandler(IAuthService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        public Task<string> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            return _authenticationService.AuthenticateAsync(request.RefreshToken, cancellationToken);
        }
    }
}
