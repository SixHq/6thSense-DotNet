using Newtonsoft.Json;
using Models;
using Sixth.RouteLogger;
namespace Sixth
{


    public class SixthSdk
    {
        private string apiKey = "";
        WebApplication application;
        ProjectConfig projectConfig;
        private const string base_url = "https://backend.withsix.co";


        // Instantiate HttpClient
        private static readonly HttpClient client = new HttpClient();
        public SixthSdk(string apiKey, WebApplication app)
        {
            this.apiKey = apiKey;
            application = app;
            Console.WriteLine(apiKey);
        }

        public async Task InitializeApp()
        {
            await GetUserConfig();

        }

        private async Task GetUserConfig()
        {
            try
            {

                var request = await client.GetAsync(base_url + "/project-config/config/" + apiKey);
                if (request.IsSuccessStatusCode)
                {
                    var responseBody = await request.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ProjectConfig>(responseBody);


                    if (application.Environment.IsDevelopment())
                    {
                        application.MapGet("/debug", GetAllEndpoint);
                    }
                    application.UseRouteLogger_Middleware();


                

                   var responseT = await client.GetAsync("http://localhost:5280/api/path");
                   if (responseT.IsSuccessStatusCode)
                   {
                       Console.WriteLine("Request Successful.");
                       string responseBodyT = await responseT.Content.ReadAsStringAsync();
                       Console.WriteLine(responseBody);
                   }
                   else
                   {
                       Console.WriteLine($"Request Failed with status code: {responseT.StatusCode}");
                   }
//

                }
                else
                {
                    //error ocured 

                }


            }
            catch (System.Exception)
            {

                throw;
            }

        }

        private async Task GetAllEndpoint(IEnumerable<EndpointDataSource> endpointSources)
        {
            Console.WriteLine("hhhhhhh");

        }

        private async Task SyncProjectRoute()
        {
            //sync the config with db
            application.Configuration.Get<Route>();

        }


    }




}
