using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
        }

        private async void btnListClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clients = await Client.GetAllClient();

                ListeClient listeClient = new ListeClient(clients);
                this.Content = listeClient;
                this.Show();
            } catch (Exception error)
            {
                lbl_error_liste_client.Content = error.Message;
            }
        }
        
        private void btnListModule_Click(object sender, RoutedEventArgs e)
        {

            ListeModule listeModule = new ListeModule();
            this.Content = listeModule;
            this.Show();
        }
    }
}
