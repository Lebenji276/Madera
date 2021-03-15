using JustEat.HttpClientInterception;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;

namespace Madera
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpClient httpClient;

        protected override void OnStartup(StartupEventArgs e)
        {
            var options = new HttpClientInterceptorOptions();

            var builder = new HttpRequestInterceptionBuilder()
                .Requests()
                .ForHost("localhost")
                .RegisterWith(options);

            var client = options.CreateHttpClient();

            httpClient = client;

            base.OnStartup(e);
        }

        public static void setBearer(string bearerToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", bearerToken);
        }
    }
}
