
namespace Sixth.RouteLogger
{

    public class RouteLogger_Middleware
    {

        private readonly RequestDelegate _next;
        public RouteLogger_Middleware(RequestDelegate next)
        {
            _next = next;
        }




        public async Task Invoke(HttpContext httpContext)
        {
            //get the api key

            var request = httpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            var scheme = request.Scheme;

            var baseUrl = $"{scheme}://{host}{pathBase}";

            Console.WriteLine(baseUrl);
            await _next.Invoke(httpContext);
        }

    }



    public static class RouteLogger_Middleware_MiddlewareExtensions
    {
        public static IApplicationBuilder UseRouteLogger_Middleware(this IApplicationBuilder builder)
        {

            return builder.UseMiddleware<RouteLogger_Middleware>();


        }

    }

}