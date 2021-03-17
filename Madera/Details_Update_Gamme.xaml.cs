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
    /// Logique d'interaction pour Details_Update_Gamme.xaml
    /// </summary>
    public partial class Details_Update_Gamme : Window
    {
        Gamme _obj;
        Module[] _modules;
        public Details_Update_Gamme(Gamme obj, Module[] modules)
        {
            InitializeComponent();
            _obj = obj;
            _modules = modules;
            SetValue();
            this.Show();
        }
        public void SetValue()
        {
            
            Detail_module_value_name.Text = _obj.nomGamme;
            Detail_module_value_Description.Text = _obj.description;
            ListModulesGamme.ItemsSource = "";
            ListModulesGamme.ItemsSource = _obj.listmodules;
            ListModules.ItemsSource = "";

            ListModules.ItemsSource = _modules;
        }
        private void ClickQuit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ClickModify(object sender, RoutedEventArgs e)
        {
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
            _obj.module = newJson;
            _obj.module = JsonConvert.SerializeObject(_obj.listmodules);

            Gamme.UpdateGamme(_obj);
            Close();
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
            _obj.listmodules.Add(moduleSelect);
            SetValue();
        }

        private void removeModule(object sender, RoutedEventArgs e)
        {
            Module moduleSelect = (Module)ListModulesGamme.SelectedItem;
            _obj.listmodules.Remove(moduleSelect);
            SetValue();

        }
    }
}
