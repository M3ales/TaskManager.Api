using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Exceptions;
using TaskManager.Api.Application.Common.Interfaces;
using TaskManager.Api.Domain.Entities.Auth;
using Claim = System.Security.Claims.Claim;

namespace Infrastructure.Authentication
{
    public class AuthService : IAuthService
    {
		private readonly IApplicationDbContext _applicationDbContext;
		public AuthService(IApplicationDbContext applicationDbContext)
        {
			_applicationDbContext = applicationDbContext;

		}

        public Task<string> AuthenticateAsync(string refreshToken, CancellationToken cancellationToken)
        {
			var account = _applicationDbContext.Accounts
				.Include(account => account.Claims)
				.FirstOrDefault(account => account.RefreshToken == refreshToken) 
				?? throw new NotFoundException("Token", refreshToken);
			
			return Task.FromResult(GenerateToken(account, 3600));
        }

		public string GenerateToken(Account account, int validForSeconds)
		{
			// From: https://dotnetcoretutorials.com/2020/01/15/creating-and-validating-jwt-tokens-in-asp-net-core/
			var mySecret = "wesfaeioj12j3pk;akDAs;m12dmasdm2km";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			var myIssuer = "TaskManager.Api";
			var myAudience = "TaskManager.Api";

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
				}),
				Expires = DateTime.UtcNow.AddSeconds(validForSeconds),
				Issuer = myIssuer,
				Audience = myAudience,
				SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature),
				Claims = account.Claims.ToDictionary(key => key.Name, value => (object)value.Value)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public bool ValidateCurrentToken(string token)
		{
			// From: https://dotnetcoretutorials.com/2020/01/15/creating-and-validating-jwt-tokens-in-asp-net-core/
			var mySecret = "wesfaeioj12j3pk;akDAs;m12dmasdm2km";
			var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

			var myIssuer = "http://mysite.com";
			var myAudience = "http://myaudience.com";

			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = myIssuer,
					ValidAudience = myAudience,
					IssuerSigningKey = mySecurityKey
				}, out SecurityToken validatedToken);
			}
			catch
			{
				return false;
			}
			return true;
		}
	}
}
