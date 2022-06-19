using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ValidateConnection
{
    public static class ValidateClient
    {
        private static HttpClient HttpClientObj = new HttpClient();
        [FunctionName("Function1")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;
            var response = await HttpClientObj.GetAsync(Environment.GetEnvironmentVariable("QueueFunctionURL"));
            var responseContent = await response.Content.ReadAsStringAsync();
            
            return string.IsNullOrEmpty(responseContent) 
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Response is empty.")
                : req.CreateResponse(HttpStatusCode.OK, responseContent);
        }
    }
}
