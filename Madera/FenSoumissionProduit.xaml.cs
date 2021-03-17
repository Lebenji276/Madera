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

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            MenuWindow menu = new MenuWindow();
            App.Current.MainWindow = menu;
            Close();
            menu.Show();
        }
        private void OpenModule(object sender, RoutedEventArgs e)
        {
            FenCreationModule main = new FenCreationModule();
            App.Current.MainWindow = main;
            main.Show();
        }

        private void OpenComposant(object sender, RoutedEventArgs e)
        {
            FenCreationComposant main = new FenCreationComposant();
            App.Current.MainWindow = main;
            main.Show();
        }

        private void OpenGamme(object sender, RoutedEventArgs e)
        {
            FenCreationGamme main = new FenCreationGamme();
            App.Current.MainWindow = main;
            main.Show();
        }
    }
}
