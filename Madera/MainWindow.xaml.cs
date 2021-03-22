using Madera.Classe;
using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            addImage();
        }

        private void addImage()
        {
            var image = new Image();
            image.HorizontalAlignment = HorizontalAlignment.Left;
            image.Height = 100;
            image.Margin = new Thickness(32, 22, 0, 0);
            image.VerticalAlignment = VerticalAlignment.Top;

            string currentDir = Directory.GetCurrentDirectory();
            string[] currentDirSplitted = currentDir.Split("\\bin");
            var uriSource = new Uri(currentDirSplitted[0] + "\\madera.png", UriKind.Absolute);

            image.Source = new BitmapImage(uriSource);
            image.Width = 350;
            grid_principal.Children.Add(image);
        }

        private void createDevis(object sender, RoutedEventArgs e)
        {
            Devis devis = new Devis();
            Client client = new Client();
            client.first_name = "Léandre";
            devis.client = client._id;
            devis.nomProjet = "projet test";
            Devis.CreateDevis(devis);

        }
        private void resetErrors()
        {
            lbl_error_login.Content = "";
            lbl_error_password.Content = "";
        }

        async void OnClickValidate(object sender, RoutedEventArgs e)
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

            try
            {
                // HERE Faire la synchro via api et locale si pas de co
                if (App.haveConnection)
                {
                    var user = await Auth.postAuth(username, password);

                    if (user == null)
                    {
                        throw new Exception("Impossible de connecter l'utilisateur");
                    }

                    await User.resetUserJSON(username);
                    Console.WriteLine(user);
                } else
                {
                    if (Auth.checkAuth(username, password))
                    {
                        var user = User.getUser(username);
                        Console.WriteLine(user);
                        App.user = user;
                    }
                    else
                    {
                        throw new Exception("Impossible de connecter l'utilisateur");
                    }
                }

          
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
