using Madera.Classe;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Madera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static readonly HttpClient client = new HttpClient();

        static async Task Authentication()
        {
            try
            {
                HttpResponseMessage response = await client.PostAsync("http://localhost:5000/auth/login?username=leandreg&password=string",
                                                                      new StringContent(""));
                string responseBody = JsonSerializer.Serialize(await response.Content.ReadAsStringAsync());
                Console.WriteLine(responseBody);
            } catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        public class Product
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }
        }

        private void resetErrors()
        {
            lbl_error_login.Content = "";
            lbl_error_password.Content = "";
        }

        async void OnClick1(object sender, RoutedEventArgs e)
        {
            int errors = 0;
            String username = textBox.Text;
            String password = passwordBox.Password;

            this.resetErrors();

            if (String.IsNullOrEmpty(username))
            {
                lbl_error_login.Content = "Veuillez renseigner un login";
                errors++;
                return;
            }

            if (String.IsNullOrEmpty(password))
            {
                lbl_error_password.Content = "Veuillez renseigner un mot de passe";
                errors++;
                return;
            }

            label_1.Content = "User : " + username;
            label_2.Content = "MDP : " + password;
            try
            {
                Auth auth = await Auth.postAuth(username, password);

                MenuWindow main = new MenuWindow();
                App.Current.MainWindow = main;
                this.Close();
                main.Show();
            } catch (Exception error)
            {
                lbl_error_login.Content = error.Message;
            }
            
        }

    }
}
