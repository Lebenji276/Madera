﻿using Madera.Classe;
using System;
using System.Windows;

namespace Madera
{
    /// <summary>
    /// Logique d'interaction pour FenCreationDevis.xaml
    /// </summary>
    public partial class FenCreationDevis : Window
    {
        public FenCreationDevis()
        {
            InitializeComponent();
            var toto = Client.GetAllClient();
            ComboClients.ItemsSource = toto.Result;
            var modules = Module.GetAllModule();
            ListeModules.ItemsSource = modules.Result;
            ComModules.ItemsSource = modules.Result;
            ComboComposants.Items.Add("Angle entrant");
            ComboComposants.Items.Add("Angle sortant");
            //try
            //{
                var gammes = Gamme.GetAllGammes();
                ComboGammes.ItemsSource = gammes.Result;
            //}
            //catch (AggregateException e)
            //{
              //  Console.WriteLine(e);
            //}
            
        }
    }
}