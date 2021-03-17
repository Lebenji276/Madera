using Madera.Classe;
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

        public void initialiseListView()
        {
            var composants = Composant.GetAllComposant();
            ListComposants.ItemsSource = composants.Result;
        }

        private void AddComposant(object sender, RoutedEventArgs e)
        {
            Composant composantSelect = (Composant)ListComposants.SelectedItem;
            string nomComposant = ListComposants.SelectedValue.ToString();
            foreach (Composant composant in ListComposantsModule.Items)
            {
                if (composant.nomComposant == nomComposant)
                {
                    MessageBox.Show("Ce module est déjà utilisé.");
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
            Module module = new Module();
            int i = 0;
            int taille = ListComposantsModule.Items.Count;
            //List<Composant> listModules = new List<Composant>();
            Composant[] tabComposants = new Composant[taille];
            for (i=0;i<taille; i++)
            {
                tabComposants[i] = (Composant)ListComposantsModule.Items[i];
            }
            module.composants = tabComposants;
            module.nomModule = TextBoxName.Text;
            module.description = TextBoxDescription.Text;
        }
    }
}
