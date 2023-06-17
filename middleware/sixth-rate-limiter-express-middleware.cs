using System.Text;
using AspNetCore.RouteAnalyzer;
public class Sixth_Rate_Limiter_Express_Middleware
{
    private readonly RequestDelegate _next;



    private Sixth_Rate_Limiter_Express_Middleware(RequestDelegate next)
    {
        _next = next;
        

    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var logDict = new Dictionary<string, List<string>>();
        var route = httpContext.Request.Path.Value;
        route = FormatRoute(route);

        var host = httpContext.Request.Host;
        var apiKey = httpContext.Request.Headers["apikey"];
        var  rate_limit_resp = GetFromFirebaseStorageAsync("https://backend.withsix.co/project-config/config/get-route-rate-limit/", apiKey,route);
        if(rate_limit_resp.IsCompletedSuccessfully)
        {

        }
        else
        {
            
        }

        
      
        
       
    }


    private async Task<string> GetFromFirebaseStorageAsync(string fileUrl, string apiKey,string route)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync($"{fileUrl}{apiKey}/{route}");;

        if (response.IsSuccessStatusCode)
        {
            // Read and return the response content as a string
            return await response.Content.ReadAsStringAsync();
        }
        throw new Exception($"Error retrieving text file: {response.ReasonPhrase}");
    }

    private string  FormatRoute(string url)
    {
        StringBuilder stringBuilder = new StringBuilder(url) ;
        stringBuilder.Replace("/","~");
        return stringBuilder.ToString();
    }
    
    private bool IsRateLimitReached(Dictionary<string,string> routeData, string uID)
    {

        if(routeData.ContainsKey(uID))
        {
            
        }
        else
        {

        }
        return false;


    }

       



}


public static class Sixth_Rate_Limiter_Express_MiddlewareExtensions
{
    public static IApplicationBuilder UseSixth_Rate_Limiter_Express_Middleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<Sixth_Rate_Limiter_Express_Middleware>();

    }

}