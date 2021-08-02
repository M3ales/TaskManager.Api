using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Api.Application.Common.Interfaces;

namespace WebApi.Services
{
    public class HttpHeaderJwtService : IRequestJwtService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpHeaderJwtService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string Token
        {
            get
            {
                var header = _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"].ToString() ?? "";
                if (string.IsNullOrWhiteSpace(header)) return string.Empty;
                return header.Replace("Bearer ", "").Trim();
            }
        }
    }
}
