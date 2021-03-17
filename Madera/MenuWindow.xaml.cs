using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

            if (App.haveConnection)
                btn_synchro.IsEnabled = true;
            else 
                btn_synchro.IsEnabled = false;
        }

        private async void btnListClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clients = await Client.GetAllClient();

                ListeClientWindow listeClient = new ListeClientWindow(clients);
                App.Current.MainWindow = listeClient;
                this.Close();
                listeClient.Show();
            }
            catch (Exception error)
            {
                lbl_error_liste_client.Content = error.Message;
            }
        }

        private async void btnListModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var modules = await Module.GetAllModule();

                ListeModuleWindow listeModule = new ListeModuleWindow(modules);
                App.Current.MainWindow = listeModule;
                this.Close();
                listeModule.Show();
            }
            catch (Exception error)
            {
                lbl_error_liste_client.Content = error.Message;
            }
        }

        private void btnAgenda_Click(object sender, RoutedEventArgs e)
        {
            AgendaWindow agenda = new AgendaWindow();
            App.Current.MainWindow = agenda;
            this.Close();
            agenda.Show();
        }
    }
}
