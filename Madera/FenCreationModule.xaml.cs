﻿using Madera.Classe;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Logique d'interaction pour FenCreationModule.xaml
    /// </summary>
    public partial class FenCreationModule : Window
    {
        public FenCreationModule()
        {
            InitializeComponent();
            initialiseListView();
        }

        public bool testSaisie()
        {
            if (TextBoxName.Text == "")
            {
                MessageBox.Show("Veuillez entrer le nom du module.");
                return false;
            }
            if(TextBoxDescription.Text == "")
            {
                MessageBox.Show("Veuillez entrer la description du module.");
                return false;
            }
            if (ListComposantsModule.Items.Count == 0)
            {
                MessageBox.Show("Veuillez entrer au moins un composant.");
                return false;
            }
            return true;
        }

        public void initialiseListView()
        {
            var composants = Composant.GetAllComposant();
            ListComposants.ItemsSource = composants.Result;
        }

        public void resetFields()
        {
            TextBoxName.Text = "";
            TextBoxDescription.Text = "";
            ListComposantsModule.Items.Clear();
            MessageBox.Show("Module créé.");
        }

        private void AddComposant(object sender, RoutedEventArgs e)
        {
            Composant composantSelect = (Composant)ListComposants.SelectedItem;
            string nomComposant = ListComposants.SelectedValue.ToString();
            foreach (Composant composant in ListComposantsModule.Items)
            {
                if (composant.nomComposant == nomComposant)
                {
                    MessageBox.Show("Ce composant est déjà utilisé.");
                    return;
                }
            }
            ListComposantsModule.Items.Add(composantSelect);
        }

        private void removeComposant(object sender, RoutedEventArgs e)
        {
            if (ListComposantsModule.SelectedIndex != -1)
            {
                ListComposantsModule.Items.RemoveAt(ListComposantsModule.SelectedIndex);
            }
        }

        private void createModule(object sender, RoutedEventArgs e)
        {
            if (testSaisie() == false)
            {
                return;
            }
            Module module = new Module();
            int i = 0;
            int taille = ListComposantsModule.Items.Count;
            Composant[] tabComposants = new Composant[taille];
            String[] tabId = new string[taille];
            List<String> listestr = new List<String>();
            foreach(Composant composant in ListComposantsModule.Items)
            {
                listestr.Add(composant._id);
            }
            for (i=0;i<taille; i++)
            {
                tabComposants[i] = (Composant)ListComposantsModule.Items[i];
            }
            String[] tab = new String[taille];
            string newJson = JsonConvert.SerializeObject(listestr, Formatting.None);
            module.composant = newJson;
            module.nomModule = TextBoxName.Text;
            module.description = TextBoxDescription.Text;
            Module.CreateModule(module);
            resetFields();
        }
    }
}