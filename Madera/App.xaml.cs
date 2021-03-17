using JustEat.HttpClientInterception;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using MaterialDesignThemes.Wpf;
using System.Windows.Media;
using Madera.Classe;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
using System.Threading;
using System.Timers;

namespace Madera
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static HttpClient httpClient;
        public static bool haveConnection;
        internal static User user;

        protected override void OnStartup(StartupEventArgs e)
        {
            SetTheme();

            var options = new HttpClientInterceptorOptions();

            var client = options.CreateHttpClient();

            httpClient = client;

            var builder = new HttpRequestInterceptionBuilder()
                .Requests()
                .ForAnyHost()
                .ForPath("*")
                .RegisterWith(options);

            base.OnStartup(e);

            checkConnexion();

            if (haveConnection)
            {
                var Synchro = new Synchro();
            }

            configureTimerConnexion();
            
        }

        public static void configureTimerConnexion()
        {
            //TimeSpan.FromMinutes(1).TotalMilliseconds
            System.Timers.Timer timer = new System.Timers.Timer(1000);
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(checkConnexionEvery);
            timer.Start();
        }

        public static void checkConnexionEvery(object sender, ElapsedEventArgs e)
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                haveConnection = (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                haveConnection = false;
            }
        }

 /*       public static void changeBtn()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MenuWindow))
                {
                    (window as MenuWindow).btn_synchro.IsEnabled = haveConnection;
                }
            }
        }*/

        public void checkConnexion()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                haveConnection = (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                haveConnection = false;
            }
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
