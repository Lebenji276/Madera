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

        public MainWindow()
        {
            InitializeComponent();
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
