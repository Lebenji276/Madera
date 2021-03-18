using Madera.Classe;
using Newtonsoft.Json;
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
    /// Logique d'interaction pour FenCreationGamme.xaml
    /// </summary>
    public partial class FenCreationGamme : Window
    {
        public FenCreationGamme()
        {
            InitializeComponent();
            initialiseListView();
        }

        public async void initialiseListView()
        {
            var modules = await Module.GetAllModule();
            ListModules.ItemsSource = modules;
        }

        public bool testSaisie()
        {
            if (TextBoxName.Text == "")
            {
                MessageBox.Show("Veuillez entrer le nom de la gamme.");
                return false;
            }
            if (TextBoxDescription.Text == "")
            {
                MessageBox.Show("Veuillez entrer la description de la gamme.");
                return false;
            }
            if (ListModulesGamme.Items.Count == 0)
            {
                MessageBox.Show("Veuillez entrer au moins un module.");
                return false;
            }
            return true;
        }

        public void resetFields()
        {
            TextBoxName.Text = "";
            TextBoxDescription.Text = "";
            ListModulesGamme.Items.Clear();
            MessageBox.Show("Gamme créée.");
        }

        private void AddModule(object sender, RoutedEventArgs e)
        {
            Module moduleSelect = (Module)ListModules.SelectedItem;
            string nomModule = ListModules.SelectedValue.ToString();
            foreach (Module module in ListModulesGamme.Items)
            {
                if (module.nomModule == nomModule)
                {
                    MessageBox.Show("Ce module est déjà utilisé.");
                    return;
                }
            }
            ListModulesGamme.Items.Add(moduleSelect);
        }

        private void removeModule(object sender, RoutedEventArgs e)
        {
            if (ListModulesGamme.SelectedIndex != -1)
            {
                ListModulesGamme.Items.RemoveAt(ListModulesGamme.SelectedIndex);
            }
        }

        private async void createGamme(object sender, RoutedEventArgs e)
        {
            if (testSaisie() == false)
            {
                return;
            }
            Gamme gamme = new Gamme();
            int i = 0;
            int taille = ListModulesGamme.Items.Count;
            Module[] tabModules = new Module[taille];
            String[] tabId = new string[taille];
            List<String> listestr = new List<String>();
            foreach (Module module in ListModulesGamme.Items)
            {
                listestr.Add(module._id);
            }
            for (i = 0; i < taille; i++)
            {
                tabModules[i] = (Module)ListModulesGamme.Items[i];
            }
            String[] tab = new String[taille];
            string newJson = JsonConvert.SerializeObject(listestr, Formatting.None);
            gamme.module = newJson;
            gamme.nomGamme= TextBoxName.Text;
            gamme.description = TextBoxDescription.Text;
            await Gamme.CreateGamme(gamme);
            resetFields();
        }
    }
}
