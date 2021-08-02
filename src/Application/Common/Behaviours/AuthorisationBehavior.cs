using MediatR;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Attributes;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;

namespace TaskManager.Api.Application.Common.Behaviours
{
    public class AuthorisationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IAuthService _authService;
        private readonly IRequestJwtService _requestAuthService;
        public AuthorisationBehaviour(IRequestJwtService requestAuthService, IAuthService authService)
        {
            _authService = authService;
            _requestAuthService = requestAuthService;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var authZAttributes = request.GetType().GetCustomAttributes<AuthoriseAttribute>();

            if (authZAttributes.Any())
            {
                var jwt = _requestAuthService.Token;
                if (!_authService.ValidateCurrentToken(jwt)) throw new UnauthorizedAccessException();
                // Check for claims
                // then throw new ForbiddenAccessException();
            }

            return await next();
        }
    }
}
