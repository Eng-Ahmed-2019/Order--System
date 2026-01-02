using OrderSystem.Application.Interfaces;

namespace OrderSystem.API.Middlewares
{
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public SessionValidationMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context,ISessionRepository repository)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var sid = context.User.FindFirst("sid")?.Value;
                if (sid == null)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
                var session = await repository.GetByIdAsync(Guid.Parse(sid));
                if (session == null || session.ExpiresAt < DateTime.UtcNow)
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            await _requestDelegate(context);
        }
    }
}