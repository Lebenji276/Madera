using Madera.Classe;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
    /// Logique d'interaction pour FenSoumissionProduit.xaml
    /// </summary>
    public partial class FenSoumissionProduit : Window
    {
        public FenSoumissionProduit()
        {
            InitializeComponent();
        }

        public void resetFields()
        {
            TextBoxName.Text = "";
            TextBoxDescription.Text = "";
            MessageBox.Show("Produit enregistré.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (RadioGamme.IsChecked == true)
            {
                Gamme gamme = new Gamme();
                gamme.nomGamme = TextBoxName.Text;
                gamme.description = TextBoxDescription.Text;
                Gamme.CreateGamme(gamme);
                resetFields();
            }
            if (RadioModule.IsChecked == true)
            {
                Module module = new Module();
                module.nomModule = TextBoxName.Text;
                module.description = TextBoxDescription.Text;
                Module.CreateModule(module);
                resetFields();
            }
            if (RadioComposant.IsChecked == true)
            {
                Composant composant = new Composant();
                composant.nomComposant = TextBoxName.Text;
                composant.description = TextBoxDescription.Text;
                Composant.CreateComposant(composant);
                resetFields();
            }
        }
    }
}
