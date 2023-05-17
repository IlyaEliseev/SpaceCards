namespace SpaceCards.API
{
    public class JwtFromCookieMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtFromCookieMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["_sp_i"];

            if (!string.IsNullOrWhiteSpace(token))
            {
                context.Request.Headers.Authorization = "Bearer " + token;
            }

            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Add("X-Xss-Protection", "1");
            context.Response.Headers.Add("X-Frame-Options", "DENY");

            await _next(context);
        }
    }
}
