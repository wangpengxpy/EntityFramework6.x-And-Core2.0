using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EntityFrameworkBaseExample.Controllers
{
    public class PollyController : ApiController
    {
        public readonly RetryPolicy<HttpResponseMessage> _httpRequestPolicy;
        public PollyController()
        {
            _httpRequestPolicy = Policy.HandleResult<HttpResponseMessage>(
            r => r.StatusCode == HttpStatusCode.InternalServerError)
            .WaitAndRetryAsync(3,
            retryAttempt => TimeSpan.FromSeconds(retryAttempt));
        }

        public async Task<IHttpActionResult> Get()
        {
            var httpClient = new HttpClient();
            string requestEndpoint = "http://localhost:4096";

            HttpResponseMessage httpResponse = await _httpRequestPolicy.ExecuteAsync(() => httpClient.GetAsync(requestEndpoint));

            IEnumerable<string> numbers = await httpResponse.Content.ReadAsAsync<IEnumerable<string>>();

            return Ok(numbers);
        }
    }
}
