using JustEat.HttpClientInterception;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;

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

            SetTheme();
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

        public static void SetTheme()
        {
            Color primaryColor = Colors.Black;
            Color secondaryColor = Colors.Gray;

            IBaseTheme baseTheme = Theme.Light;
            //If you want a dark theme you can use IBaseTheme baseTheme = Theme.Dark;
            ITheme theme = Theme.Create(baseTheme, primaryColor, secondaryColor);
            ResourceDictionaryExtensions.SetTheme(Application.Current.Resources,theme);
        }
    }
}
