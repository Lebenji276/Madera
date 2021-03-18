﻿using Madera.Classe;
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

            label.Content = "Bonjour " + App.user.prenom + " " + App.user.nom;

            if (App.haveConnection)
                btn_synchro.IsEnabled = true;
            else 
                btn_synchro.IsEnabled = false;
        }

        private void btnListClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clients = Client.GetAllClient();

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

        private void btnListModule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var modules = Module.GetAllModule();

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

        void OnClick1(object sender, RoutedEventArgs e)
        {
            FenSoumissionProduit main = new FenSoumissionProduit();
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }

        void  OnClick2(object sender, RoutedEventArgs e)
        {
            var modules = Module.GetAllModule();
            FenCreationDevis main = new FenCreationDevis(modules);
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }

        private void btnListGamme_Click(object sender, RoutedEventArgs e)
        {
            var modules = Module.GetAllModule();

            ListeGamme listeGamme = new ListeGamme(modules);
            App.Current.MainWindow = listeGamme;
            this.Close();
            listeGamme.Show();
        }

        async void btnListeComposantClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var composants = await Composant.GetAllComposant();

                ListeComposantWindow main = new ListeComposantWindow(composants);
                App.Current.MainWindow = main;
                this.Close();
                main.Show();
            }
            catch (Exception error)
            {
                lbl_error_liste_client.Content = error.Message;
            }
        }

        private void btn_synchro_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
