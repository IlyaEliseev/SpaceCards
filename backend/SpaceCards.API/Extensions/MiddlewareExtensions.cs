namespace SpaceCards.API.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtFromCookie(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<JwtFromCookieMiddleware>();
        }
    }
}
